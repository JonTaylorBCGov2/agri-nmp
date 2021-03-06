﻿using System;
using System.Collections.Generic;
using System.Linq;
using Agri.Models;
using Agri.Models.Calculate;
using Agri.Models.Farm;

namespace Agri.CalculateService
{
    public interface IManureApplicationCalculator
    {
        AppliedManure GetAppliedManure(YearData yearData, FarmManure farmManure);

        AppliedStoredManure GetAppliedManureFromStorageSystem(YearData yearData, ManureStorageSystem manureStorageSystem);

        AppliedStoredManure GetAppliedStoredManure(YearData yearData, FarmManure farmManure);

        AppliedImportedManure GetAppliedImportedManure(YearData yearData, FarmManure farmManure);
    }

    public class ManureApplicationCalculator : IManureApplicationCalculator
    {
        private IManureUnitConversionCalculator _manureUnitConversionCalculator;

        public ManureApplicationCalculator(IManureUnitConversionCalculator manureUnitConversionCalculator)
        {
            _manureUnitConversionCalculator = manureUnitConversionCalculator;
        }

        public AppliedManure GetAppliedManure(YearData yearData, FarmManure farmManure)
        {
            AppliedManure appliedManure = null;
            if (farmManure.StoredImported == NutrientAnalysisTypes.Stored)
            {
                //Stored Manure
                appliedManure = GetAppliedStoredManure(yearData, farmManure);
            }
            else if (farmManure.StoredImported == NutrientAnalysisTypes.Imported)
            {
                appliedManure = GetAppliedImportedManure(yearData, farmManure);
            }

            return appliedManure;
        }

        public AppliedStoredManure GetAppliedStoredManure(YearData yearData, FarmManure farmManure)
        {
            var manureStorageSystem = yearData.ManureStorageSystems.SingleOrDefault(mss => mss.Id == farmManure.SourceOfMaterialStoredSystemId);
            var appliedStoredManure = GetAppliedManureFromStorageSystem(yearData, manureStorageSystem);

            return appliedStoredManure;
        }

        public AppliedStoredManure GetAppliedManureFromStorageSystem(YearData yearData, ManureStorageSystem manureStorageSystem)
        {
            var fieldsAppliedWithStoredManure = yearData.GetFieldsAppliedWithManure(manureStorageSystem);
            var farmManureIds = yearData.GetFarmManureIds(manureStorageSystem);

            var fieldAppliedManures = new List<FieldAppliedManure>();
            foreach (var field in fieldsAppliedWithStoredManure)
            {
                var nutrientManures = field?.Nutrients.nutrientManures
                    .Where(nm => farmManureIds.Any(fm => fm == Convert.ToInt32(nm.manureId)))
                    .ToList() ?? new List<NutrientManure>();

                foreach (var nutrientManure in nutrientManures)
                {
                    var fieldAppliedManure = new FieldAppliedManure();
                    if (manureStorageSystem.ManureMaterialType == ManureMaterialType.Liquid)
                    {
                        var convertedRate = _manureUnitConversionCalculator
                            .GetLiquidUSGallonsPerAcreApplicationRate(nutrientManure.rate,
                                (ApplicationRateUnits)Convert.ToInt32(nutrientManure.unitId));

                        fieldAppliedManure.USGallonsApplied =
                            field.Area * convertedRate;
                    }
                    else
                    {
                        var farmManure = yearData.FarmManures
                            .Single(fm => fm.Id == Convert.ToInt32(nutrientManure.manureId));

                        decimal convertedRate;
                        if (string.IsNullOrWhiteSpace(farmManure.Moisture))
                        {
                            convertedRate = _manureUnitConversionCalculator
                                .GetSolidsTonsPerAcreApplicationRate(farmManure.ManureId, nutrientManure.rate,
                                    (ApplicationRateUnits)Convert.ToInt32(nutrientManure.unitId));
                        }
                        else
                        {
                            convertedRate = _manureUnitConversionCalculator
                                .GetSolidsTonsPerAcreApplicationRate(Convert.ToDecimal(farmManure.Moisture),
                                    nutrientManure.rate,
                                    (ApplicationRateUnits)Convert.ToInt32(nutrientManure.unitId));
                        }

                        fieldAppliedManure.TonsApplied = field.Area * convertedRate;
                    }

                    fieldAppliedManures.Add(fieldAppliedManure);
                }
            }

            var appliedStoredManure = new AppliedStoredManure(fieldAppliedManures, manureStorageSystem);

            return appliedStoredManure;
        }

        public AppliedImportedManure GetAppliedImportedManure(YearData yearData, FarmManure farmManure)
        {
            var fieldsAppliedWithImportedManure = yearData.GetFieldsAppliedWithManure(farmManure);
            var importedManure = yearData.ImportedManures.SingleOrDefault(mss => mss.Id == farmManure?.SourceOfMaterialImportedManureId.Value);
            var farmManureIds = yearData.GetFarmManureIds(importedManure);

            var fieldAppliedManures = new List<FieldAppliedManure>();
            foreach (var field in fieldsAppliedWithImportedManure)
            {
                var nutrientManures = field?.Nutrients.nutrientManures
                    .Where(nm => farmManureIds.Any(fm => fm == Convert.ToInt32(nm.manureId)))
                    .ToList() ?? new List<NutrientManure>();

                foreach (var nutrientManure in nutrientManures)
                {
                    var fieldAppliedManure = new FieldAppliedManure();
                    if (importedManure.ManureType == ManureMaterialType.Liquid)
                    {
                        var convertedRate = _manureUnitConversionCalculator
                            .GetLiquidUSGallonsPerAcreApplicationRate(nutrientManure.rate,
                                (ApplicationRateUnits)Convert.ToInt32(nutrientManure.unitId));

                        fieldAppliedManure.USGallonsApplied =
                            field.Area * convertedRate;
                    }
                    else
                    {
                        var convertedRate = _manureUnitConversionCalculator
                            .GetSolidsTonsPerAcreApplicationRate(importedManure.Moisture.Value, nutrientManure.rate,
                                (ApplicationRateUnits)Convert.ToInt32(nutrientManure.unitId));

                        fieldAppliedManure.TonsApplied = field.Area * convertedRate;
                    }

                    fieldAppliedManures.Add(fieldAppliedManure);
                }
            }

            var appliedImportedManure = new AppliedImportedManure(fieldAppliedManures, importedManure);

            return appliedImportedManure;
        }
    }
}