﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Agri.Models.Configuration;
using Agri.Models.Calculate;
using Version = Agri.Models.Configuration.Version;

namespace Agri.LegacyData.Models.Impl
{
    public class StaticDataRepository
    {
        private const string MANURE_CLASS_COMPOST = "Compost";
        private const string MANURE_CLASS_COMPOST_BOOK = "Compost_Book";
        private const string MANURE_CLASS_OTHER = "Other";
        private const int CROPTYPE_GRAINS_OILSEEDS_ID = 4;
        private const int CROP_YIELD_DEFAULT_CALCULATION_UNIT = 1;
        private const int CROP_YIELD_DEFAULT_DISPLAY_UNIT = 2;


        protected JObject rss;

        public StaticDataRepository()
        {
            rss = StaticDataLoader.GetStaticDataJson();
        }

        public List<Region> GetRegions()
        {
            var regions = new List<Region>();

            JArray dataArray = (JArray) rss["agri"]["nmp"]["regions"]["region"];

            foreach (var r in dataArray)
            {
                var reg = new Region
                {
                    Id = Convert.ToInt32(r["id"].ToString()),
                    Name = r["name"].ToString(),
                    LocationId = Convert.ToInt32(r["locationid"].ToString()),
                    SoilTestPhosphorousRegionCd = Convert.ToInt32(r["soil_test_phospherous_region_cd"].ToString()),
                    SoilTestPotassiumRegionCd = Convert.ToInt32(r["soil_test_potassium_region_cd"].ToString()),
                    SortNumber = Convert.ToInt32(r["sortNum"].ToString())
                };

                regions.Add(reg);
            }

            return regions;
        }

        public List<SelectListItem> GetRegionsDll()
        {
            List<Region> regions = GetRegions();

            regions = regions.OrderBy(n => n.SortNumber).ThenBy(n => n.Name).ToList();

            var regOptions = new List<SelectListItem>();


            foreach (var r in regions)
            {
                var li = new SelectListItem()
                    {Id = r.Id, Value = r.Name};
                regOptions.Add(li);
            }

            return regOptions;
        }

        public Manure GetManure(string manId)
        {
            var man = new Models.StaticData.Manure();

            JArray manures = (JArray) rss["agri"]["nmp"]["manures"]["manure"];

            foreach (var r in manures)
            {
                if (r["id"].ToString() == manId)
                {
                    man.id = Convert.ToInt32(r["id"].ToString());
                    man.name = r["name"].ToString();
                    man.manure_class = r["manure_class"].ToString();
                    man.solid_liquid = r["solid_liquid"].ToString();
                    man.moisture = r["moisture"].ToString();
                    man.nitrogen = Convert.ToDecimal(r["nitrogen"].ToString());
                    man.ammonia = Convert.ToInt32(r["ammonia"].ToString());
                    man.phosphorous = Convert.ToDecimal(r["phosphorous"].ToString());
                    man.potassium = Convert.ToDecimal(r["potassium"].ToString());
                    man.dmid = Convert.ToInt32(r["dmid"].ToString());
                    man.nminerizationid = Convert.ToInt32(r["nminerizationid"].ToString());
                    if (man.solid_liquid.ToUpper() == "SOLID")
                        man.cubic_Yard_Conversion = Convert.ToDecimal(r["cubic_yard_conversion"].ToString());
                    else
                        man.cubic_Yard_Conversion = 0;
                    man.nitrate = Convert.ToDecimal(r["nitrate"].ToString());
                }
            }

            //return man;
            return new Manure();

        }

        public Manure GetManureByName(string manureName)
        {
            Models.StaticData.Manure man = null;

            JArray manures = (JArray) rss["agri"]["nmp"]["manures"]["manure"];
            JObject r = manures.Children<JObject>().FirstOrDefault(o => o["name"].ToString() == manureName.Trim());

            if (r != null)
            {
                man = new Models.StaticData.Manure();
                man.id = Convert.ToInt32(r["id"].ToString());
                man.name = r["name"].ToString();
                man.manure_class = r["manure_class"].ToString();
                man.solid_liquid = r["solid_liquid"].ToString();
                man.moisture = r["moisture"].ToString();
                man.nitrogen = Convert.ToDecimal(r["nitrogen"].ToString());
                man.ammonia = Convert.ToInt32(r["ammonia"].ToString());
                man.phosphorous = Convert.ToDecimal(r["phosphorous"].ToString());
                man.potassium = Convert.ToDecimal(r["potassium"].ToString());
                man.dmid = Convert.ToInt32(r["dmid"].ToString());
                man.nminerizationid = Convert.ToInt32(r["nminerizationid"].ToString());
                if (man.solid_liquid.ToUpper() == "SOLID")
                    man.cubic_Yard_Conversion = Convert.ToDecimal(r["cubic_yard_conversion"].ToString());
                else
                    man.cubic_Yard_Conversion = 0;
            }

            //return man;
            return new Manure();
        }

        public List<Manure> GetManures()
        {
            var manures = new List<Manure>();
            JArray array = (JArray) rss["agri"]["nmp"]["manures"]["manure"];

            foreach (var r in array)
            {
                var manure = new Manure
                {
                    Id = Convert.ToInt32(r["id"].ToString()),
                    Name = r["name"].ToString(),
                    ManureClass = r["manure_class"].ToString(),
                    SolidLiquid = r["solid_liquid"].ToString(),
                    Moisture = r["moisture"].ToString(),
                    Nitrogen = Convert.ToDecimal(r["nitrogen"].ToString()),
                    Ammonia = Convert.ToInt32(r["ammonia"].ToString()),
                    Phosphorous = Convert.ToDecimal(r["phosphorous"].ToString()),
                    Potassium = Convert.ToDecimal(r["potassium"].ToString()),
                    DMId = Convert.ToInt32(r["dmid"].ToString()),
                    NMineralizationId = Convert.ToInt32(r["nminerizationid"].ToString())
                };

                if (manure.SolidLiquid.ToUpper() == "SOLID")
                    manure.CubicYardConversion = Convert.ToDecimal(r["cubic_yard_conversion"].ToString());
                else
                    manure.CubicYardConversion = 0;
                manure.SortNum = Convert.ToInt32(r["sortNum"].ToString());
                manure.Nitrate = Convert.ToDecimal(r["nitrate"].ToString());
                manures.Add(manure);
            }

            return manures;
        }

        public List<SelectListItem> GetManuresDll()
        {
            var manures = GetManures();

            manures = manures.OrderBy(n => n.SortNum).ThenBy(n => n.Name).ToList();

            List<SelectListItem> manOptions = new List<SelectListItem>();

            foreach (var r in manures)
            {
                var li = new SelectListItem()
                    {Id = r.Id, Value = r.Name};
                manOptions.Add(li);
            }

            return manOptions;
        }

        public SeasonApplication GetApplication(string applId)
        {
            Models.StaticData.Season_Application appl = new Models.StaticData.Season_Application();

            JArray applications = (JArray) rss["agri"]["nmp"]["season-applications"]["season-application"];

            foreach (var r in applications)
            {
                if (r["id"].ToString() == applId)
                {
                    appl.id = Convert.ToInt32(r["id"].ToString());
                    appl.name = r["name"].ToString();
                    appl.season = r["season"].ToString();
                    appl.application_method = r["application_method"].ToString();
                    appl.dm_lt1 = Convert.ToDecimal(r["dm_lt1"].ToString());
                    appl.dm_1_5 = Convert.ToDecimal(r["dm_1_5"].ToString());
                    appl.dm_5_10 = Convert.ToDecimal(r["dm_5_10"].ToString());
                    appl.dm_gt10 = Convert.ToDecimal(r["dm_gt10"].ToString());
                    appl.poultry_solid = r["poultry_solid"].ToString();
                    appl.compost = r["season"].ToString();
                    appl.manure_type = r["manure_type"].ToString();
                }
            }

            //return appl;
            return new SeasonApplication();
        }

        public List<SeasonApplication> GetApplications()
        {
            Models.StaticData.Season_Applications appls = new Models.StaticData.Season_Applications();
            appls.season_applications = new List<Models.StaticData.Season_Application>();

            JArray applications = (JArray) rss["agri"]["nmp"]["season-applications"]["season-application"];

            foreach (var r in applications)
            {
                Models.StaticData.Season_Application appl = new Models.StaticData.Season_Application();

                appl.id = Convert.ToInt32(r["id"].ToString());
                appl.name = r["name"].ToString();
                appl.season = r["season"].ToString();
                appl.application_method = r["application_method"].ToString();
                appl.dm_lt1 = Convert.ToDecimal(r["dm_lt1"].ToString());
                appl.dm_1_5 = Convert.ToDecimal(r["dm_1_5"].ToString());
                appl.dm_5_10 = Convert.ToDecimal(r["dm_5_10"].ToString());
                appl.dm_gt10 = Convert.ToDecimal(r["dm_gt10"].ToString());
                appl.poultry_solid = r["poultry_solid"].ToString();
                appl.compost = r["season"].ToString();
                appl.sortNum = Convert.ToInt32(r["sortNum"].ToString());
                appl.manure_type = r["manure_type"].ToString();

                appls.season_applications.Add(appl);
            }

            //return appls;
            return new List<SeasonApplication>();
        }

        public List<SelectListItem> GetApplicationsDll(string manureType)
        {
            var appls = GetApplications();

            appls = appls.OrderBy(n => n.SortNum).ThenBy(n => n.Name).ToList();

            List<SelectListItem> applsOptions = new List<SelectListItem>();

            foreach (var r in appls)
            {
                if (r.ManureType.Contains(manureType))
                {
                    var li = new SelectListItem()
                        {Id = r.Id, Value = r.Name};
                    applsOptions.Add(li);
                }
            }

            return applsOptions;
        }

        public Unit GetUnit(string unitId)
        {
            Models.StaticData.Unit unit = new Models.StaticData.Unit();

            JArray units = (JArray) rss["agri"]["nmp"]["units"]["unit"];

            foreach (var r in units)
            {
                if (r["id"].ToString() == unitId)
                {
                    unit.id = Convert.ToInt32(r["id"].ToString());
                    unit.name = r["name"].ToString();
                    unit.nutrient_content_units = r["nutrient_content_units"].ToString();
                    unit.conversion_lbton = Convert.ToDecimal(r["conversion_lbton"].ToString());
                    unit.nutrient_rate_units = r["nutrient_rate_units"].ToString();
                    unit.cost_units = r["cost_units"].ToString();
                    unit.cost_applications = Convert.ToDecimal(r["cost_applications"].ToString());
                    unit.dollar_unit_area = r["dollar_unit_area"].ToString();
                    unit.value_material_units = r["value_material_units"].ToString();
                    unit.value_N = Convert.ToDecimal(r["value_N"].ToString());
                    unit.value_P2O5 = Convert.ToDecimal(r["value_P2O5"].ToString());
                    unit.value_K2O = Convert.ToDecimal(r["value_K2O"].ToString());
                    unit.solid_liquid = r["solid_liquid"].ToString();
                    unit.farm_reqd_nutrients_std_units_conversion =
                        Convert.ToDecimal(r["farm_reqd_nutrients_std_units_conversion"].ToString());
                    unit.farm_reqd_nutrients_std_units_area_conversion =
                        Convert.ToDecimal(r["farm_reqd_nutrients_std_units_area_conversion"].ToString());
                }
            }

            //return unit;
            return new Unit();
        }

        public List<Unit> GetUnits()
        {
            var units = new List<Unit>();
            JArray array = (JArray)rss["agri"]["nmp"]["units"]["unit"];
            foreach (var record in array)
            {
                var unit = new Unit
                {
                    Id = Convert.ToInt32(record["id"].ToString()),
                    Name = record["name"].ToString(),
                    NutrientContentUnits = record["nutrient_content_units"].ToString(),
                    ConversionlbTon = Convert.ToDecimal(record["conversion_lbton"].ToString()),
                    NutrientRateUnits = record["nutrient_rate_units"].ToString(),
                    CostUnits = record["cost_units"].ToString(),
                    CostApplications = Convert.ToDecimal(record["cost_applications"].ToString()),
                    DollarUnitArea = record["dollar_unit_area"].ToString(),
                    ValueMaterialUnits = record["value_material_units"].ToString(),
                    ValueN = Convert.ToDecimal(record["value_N"].ToString()),
                    ValueP2O5 = Convert.ToDecimal(record["value_P2O5"].ToString()),
                    ValueK2O = Convert.ToDecimal(record["value_K2O"].ToString()),
                    SolidLiquid = record["solid_liquid"].ToString(),
                    FarmReqdNutrientsStdUnitsConversion =
                        Convert.ToDecimal(record["farm_reqd_nutrients_std_units_conversion"].ToString()),
                    FarmReqdNutrientsStdUnitsAreaConversion =
                        Convert.ToDecimal(record["farm_reqd_nutrients_std_units_area_conversion"].ToString())
                };
                units.Add(unit);
            }

            return units;
        }

        public List<SelectListItem> GetUnitsDll(string unitType)
        {
            var units = GetUnits();

            List<SelectListItem> unitsOptions = new List<SelectListItem>();

            foreach (var r in units)
            {
                if (r.SolidLiquid == unitType)
                {
                    var li = new SelectListItem()
                        {Id = r.Id, Value = r.Name};
                    unitsOptions.Add(li);
                }
            }

            return unitsOptions;
        }

        public List<FertilizerUnit> GetFertilizerUnits()
        {
            var units = new List<FertilizerUnit>();
            JArray array = (JArray) rss["agri"]["nmp"]["fertilizerunits"]["fertilizerunit"];

            foreach (var r in array)
            {
                var unit = new FertilizerUnit
                {
                    Id = Convert.ToInt32(r["id"].ToString()),
                    Name = r["name"].ToString(),
                    DryLiquid = r["dry_liquid"].ToString(),
                    ConversionToImperialGallonsPerAcre = r["conv_to_impgalperac"] != null
                        ? Convert.ToDecimal(r["conv_to_impgalperac"].ToString())
                        : 0,
                    FarmRequiredNutrientsStdUnitsConversion = Convert.ToDecimal(r["farm_reqd_nutrients_std_units_conversion"].ToString()),
                    FarmRequiredNutrientsStdUnitsAreaConversion = Convert.ToDecimal(r["farm_reqd_nutrients_std_units_area_conversion"].ToString())
                };
            }

            return units;
        }

        public List<SelectListItem> GetFertilizerUnitsDll(string unitType)
        {
            var units = GetFertilizerUnits();

            List<SelectListItem> unitsOptions = new List<SelectListItem>();

            foreach (var r in units)
            {
                if (r.DryLiquid == unitType)
                {
                    var li = new SelectListItem()
                        {Id = r.Id, Value = r.Name};
                    unitsOptions.Add(li);
                }
            }

            return unitsOptions;
        }

        public FertilizerUnit GetFertilizerUnit(int Id)
        {
            Models.StaticData.FertilizerUnit fertilizerUnit = new Models.StaticData.FertilizerUnit();

            JArray fertilizerUnits = (JArray) rss["agri"]["nmp"]["fertilizerunits"]["fertilizerunit"];

            foreach (var r in fertilizerUnits)
            {
                if (r["id"].ToString() == Id.ToString())
                {
                    fertilizerUnit.id = Convert.ToInt32(r["id"].ToString());
                    fertilizerUnit.name = r["name"].ToString();
                    fertilizerUnit.dry_liquid = r["dry_liquid"].ToString();
                    fertilizerUnit.conv_to_impgalperac = r["conv_to_impgalperac"] == null
                        ? 0
                        : Convert.ToDecimal(r["conv_to_impgalperac"].ToString());
                    fertilizerUnit.farm_reqd_nutrients_std_units_conversion =
                        Convert.ToDecimal(r["farm_reqd_nutrients_std_units_conversion"].ToString());
                    fertilizerUnit.farm_reqd_nutrients_std_units_area_conversion =
                        Convert.ToDecimal(r["farm_reqd_nutrients_std_units_area_conversion"].ToString());
                }
            }

            //return fertilizerUnit;
            return new FertilizerUnit();
        }

        public List<DensityUnit> GetDensityUnits()
        {
            var units = new List<DensityUnit>();
            var array = (JArray) rss["agri"]["nmp"]["densityunits"]["densityunit"];

            foreach (var r in array)
            {
                var unit = new DensityUnit
                {
                    Id = Convert.ToInt32(r["id"].ToString()),
                    Name = r["name"].ToString(),
                    ConvFactor = Convert.ToDecimal(r["convfactor"].ToString())
                };
                units.Add(unit);
            }

            return units;
        }

        public List<SelectListItem> GetDensityUnitsDll()
        {
            var units = GetDensityUnits();

            List<SelectListItem> unitsOptions = new List<SelectListItem>();

            foreach (var r in units)
            {
                var li = new SelectListItem()
                    {Id = r.Id, Value = r.Name};
                unitsOptions.Add(li);
            }

            return unitsOptions;
        }

        public DensityUnit GetDensityUnit(int Id)
        {
            Models.StaticData.DensityUnit densityUnit = new Models.StaticData.DensityUnit();

            JArray densityUnits = (JArray) rss["agri"]["nmp"]["densityunits"]["densityunit"];

            foreach (var r in densityUnits)
            {
                if (r["id"].ToString() == Id.ToString())
                {
                    densityUnit.id = Convert.ToInt32(r["id"].ToString());
                    densityUnit.name = r["name"].ToString();
                    densityUnit.convfactor = Convert.ToDecimal(r["convfactor"].ToString());
                }
            }

            //return densityUnit;
            return new DensityUnit();
        }

        public List<CropType> GetCropTypes()
        {
            var types = new List<CropType>();
            JArray array = (JArray) rss["agri"]["nmp"]["croptypes"]["croptype"];

            foreach (var r in array)
            {
                var type = new CropType
                {
                    Id = Convert.ToInt32(r["id"].ToString()),
                    Name = r["name"].ToString(),
                    CoverCrop = r["covercrop"].ToString() == "true" ? true : false,
                    CrudeProteinRequired = r["crudeproteinrequired"].ToString() == "true" ? true : false,
                    CustomCrop = r["customcrop"].ToString() == "true" ? true : false,
                    ModifyNitrogen = r["modifynitrogen"].ToString() == "true" ? true : false
                };
                types.Add(type);
            }

            return types;
        }

        public CropType GetCropType(int id)
        {
            string x = id.ToString();
            JArray array = (JArray) rss["agri"]["nmp"]["croptypes"]["croptype"];
            JObject rec = array.Children<JObject>()
                .FirstOrDefault(o => o["id"] != null && o["id"].ToString() == id.ToString());

            Models.StaticData.CropType type = new Models.StaticData.CropType();
            type.id = Convert.ToInt32(rec["id"].ToString());
            type.name = rec["name"].ToString();
            type.covercrop = rec["covercrop"].ToString() == "true" ? true : false;
            type.crudeproteinrequired = rec["crudeproteinrequired"].ToString() == "true" ? true : false;
            type.customcrop = rec["customcrop"].ToString() == "true" ? true : false;
            type.modifynitrogen = rec["modifynitrogen"].ToString() == "true" ? true : false;

            //return type;
            return new CropType();
        }

        public List<SelectListItem> GetCropTypesDll()
        {
            var types = GetCropTypes();

            List<SelectListItem> typesOptions = new List<SelectListItem>();

            foreach (var r in types)
            {
                var li = new SelectListItem()
                    {Id = r.Id, Value = r.Name};
                typesOptions.Add(li);
            }

            return typesOptions;
        }

        public List<Crop> GetCrops()
        {
            var crops = new List<Crop>();

            JArray array = (JArray) rss["agri"]["nmp"]["crops"]["crop"];

            foreach (var r in array)
            {

                var crop = new Crop
                {
                    Id = Convert.ToInt32(r["id"].ToString()),
                    CropName = r["cropname"].ToString(),
                    CropTypeId = Convert.ToInt32(r["croptypeid"].ToString()),
                    YieldCd = Convert.ToInt32(r["yieldcd"].ToString()),
                    CropRemovalFactorNitrogen = r["cropremovalfactor_N"].ToString() == "null"
                        ? (decimal?) null
                        : Convert.ToDecimal(r["cropremovalfactor_N"].ToString()),
                    CropRemovalFactorP2O5 = r["cropremovalfactor_P2O5"].ToString() == "null"
                        ? (decimal?) null
                        : Convert.ToDecimal(r["cropremovalfactor_P2O5"].ToString()),
                    CropRemovalFactorK2O = r["cropremovalfactor_K2O"].ToString() == "null"
                        ? (decimal?) null
                        : Convert.ToDecimal(r["cropremovalfactor_K2O"].ToString()),
                    NitrogenRecommendationId = Convert.ToDecimal(r["n_recommcd"].ToString()),
                    NitrogenRecommendationPoundPerAcre = r["n_recomm_lbperac"].ToString() == "null"
                        ? (decimal?) null
                        : Convert.ToDecimal(r["n_recomm_lbperac"].ToString()),
                    NitrogenRecommendationUpperLimitPoundPerAcre = r["n_high_lbperac"].ToString() == "null"
                        ? (decimal?) null
                        : Convert.ToDecimal(r["n_high_lbperac"].ToString()),
                    PreviousCropCode = Convert.ToInt32(r["prevcropcd"].ToString()),
                    SortNumber = Convert.ToInt32(r["sortNum"].ToString()),
                    ManureApplicationHistory = Convert.ToInt32(r["prevyearmanureappl_volcatcd"].ToString()),
                    HarvestBushelsPerTon = r["bushelsperton"].ToString() == ""
                        ? (decimal?) null
                        : Convert.ToDecimal(r["bushelsperton"].ToString())
                };

                crops.Add(crop);
            }

            return crops;
        }

        public List<SelectListItem> GetCropsDll(int cropType)
        {
            var crops = GetCrops();

            crops = crops.OrderBy(n => n.SortNumber).ThenBy(n => n.CropName).ToList();

            List<SelectListItem> cropsOptions = new List<SelectListItem>();

            foreach (var r in crops)
            {
                if (r.CropTypeId == cropType)
                {
                    var li = new SelectListItem()
                        {Id = r.Id, Value = r.CropName};
                    cropsOptions.Add(li);
                }
            }

            return cropsOptions;
        }

        public List<Crop> GetCrops(int cropType)
        {
            var crops = GetCrops();
            foreach (var r in crops)
            {
                if (r.CropTypeId != cropType)
                {
                    crops.Remove(r);
                }
            }

            return crops;
        }

        public Crop GetCrop(int cropId)
        {
            var crops = new List<Crop>();

            JObject r = (JObject) rss["agri"]["nmp"]["crops"]["crop"]
                .FirstOrDefault(x => x["id"].ToString() == cropId.ToString());
            var crop = new Crop
            {
                CropName = r["cropname"].ToString(),
                CropTypeId = Convert.ToInt32(r["croptypeid"].ToString()),
                YieldCd = Convert.ToInt32(r["yieldcd"].ToString()),
                CropRemovalFactorNitrogen = r["cropremovalfactor_N"].ToString() == "null"
                    ? (decimal?) null
                    : Convert.ToDecimal(r["cropremovalfactor_N"].ToString()),
                CropRemovalFactorP2O5 = r["cropremovalfactor_P2O5"].ToString() == "null"
                    ? (decimal?) null
                    : Convert.ToDecimal(r["cropremovalfactor_P2O5"].ToString()),
                CropRemovalFactorK2O = r["cropremovalfactor_K2O"].ToString() == "null"
                    ? (decimal?) null
                    : Convert.ToDecimal(r["cropremovalfactor_K2O"].ToString()),
                NitrogenRecommendationId = Convert.ToDecimal(r["n_recommcd"].ToString()),
                NitrogenRecommendationPoundPerAcre = r["n_recomm_lbperac"].ToString() == "null"
                    ? (decimal?) null
                    : Convert.ToDecimal(r["n_recomm_lbperac"].ToString()),
                NitrogenRecommendationUpperLimitPoundPerAcre = r["n_high_lbperac"].ToString() == "null"
                    ? (decimal?) null
                    : Convert.ToDecimal(r["n_high_lbperac"].ToString()),
                PreviousCropCode = Convert.ToInt32(r["prevcropcd"].ToString()),
                ManureApplicationHistory = Convert.ToInt32(r["prevyearmanureappl_volcatcd"].ToString()),
                HarvestBushelsPerTon = r["bushelsperton"].ToString() == ""
                    ? (decimal?) null
                    : Convert.ToDecimal(r["bushelsperton"].ToString())
            };

            return crop;
        }

        public int GetCropPrevYearManureApplVolCatCd(int cropId)
        {
            Models.StaticData.Crops crops = new Models.StaticData.Crops();

            JObject r = (JObject) rss["agri"]["nmp"]["crops"]["crop"]
                .FirstOrDefault(x => x["id"].ToString() == cropId.ToString());

            return Convert.ToInt32(r["prevyearmanureappl_volcatcd"].ToString());
        }

        public List<Yield> GetYield(int yieldId)
        {
            JArray array = (JArray) rss["agri"]["nmp"]["yields"]["yield"];
            var yields = new List<Yield>();

            foreach (var r in array)
            {
                var yield = new Yield
                {
                    Id = Convert.ToInt32(r["id"].ToString()),
                    YieldDesc = r["yielddesc"].ToString()
                };
                yields.Add(yield);
            }

            return yields;
        }

        public CropSoilTestPhosphorousRegion GetCropSTPRegionCd(int cropid, int soil_test_phosphorous_region_cd)
        {

            JArray array = (JArray) rss["agri"]["nmp"]["crop_stp_regioncds"]["crop_stp_regioncd"];
            Models.StaticData.CropSTPRegionCd crop_stp_regioncd = new Models.StaticData.CropSTPRegionCd();

            foreach (var r in array)
            {
                if (Convert.ToInt32(r["cropid"].ToString()) == cropid &&
                    Convert.ToInt32(r["soil_test_phosphorous_region_cd"].ToString()) == soil_test_phosphorous_region_cd)
                {
                    crop_stp_regioncd.cropid = Convert.ToInt32(r["cropid"].ToString());
                    crop_stp_regioncd.soil_test_phosphorous_region_cd =
                        Convert.ToInt32(r["soil_test_phosphorous_region_cd"].ToString());
                    crop_stp_regioncd.phosphorous_crop_group_region_cd =
                        r["phosphorous_crop_group_region_cd"].ToString() == "null"
                            ? (int?) null
                            : Convert.ToInt32(r["phosphorous_crop_group_region_cd"].ToString());
                }
            }

            //return crop_stp_regioncd;
            return new CropSoilTestPhosphorousRegion();
        }

        public CropSoilTestPotassiumRegion GetCropSTKRegionCd(int cropid, int soil_test_potassium_region_cd)
        {

            JArray array = (JArray) rss["agri"]["nmp"]["crop_stk_regioncds"]["crop_stk_regioncd"];
            Models.StaticData.CropSTKRegionCd crop_stk_regioncd = new Models.StaticData.CropSTKRegionCd();

            foreach (var r in array)
            {
                if (Convert.ToInt32(r["cropid"].ToString()) == cropid &&
                    Convert.ToInt32(r["soil_test_potassium_region_cd"].ToString()) == soil_test_potassium_region_cd)
                {
                    crop_stk_regioncd.cropid = Convert.ToInt32(r["cropid"].ToString());
                    crop_stk_regioncd.soil_test_potassium_region_cd =
                        Convert.ToInt32(r["soil_test_potassium_region_cd"].ToString());
                    crop_stk_regioncd.potassium_crop_group_region_cd =
                        r["potassium_crop_group_region_cd"].ToString() == "null"
                            ? (int?) null
                            : Convert.ToInt32(r["potassium_crop_group_region_cd"].ToString());
                }
            }

            //return crop_stk_regioncd;
            return new CropSoilTestPotassiumRegion();
        }

        public DryMatter GetDryMatter(int ID)
        {

            JArray array = (JArray) rss["agri"]["nmp"]["dms"]["dm"];
            Models.StaticData.DM dm = new Models.StaticData.DM();

            foreach (var r in array)
            {
                if (Convert.ToInt32(r["ID"].ToString()) == ID)
                {
                    dm.ID = Convert.ToInt32(r["ID"].ToString());
                    dm.name = r["name"].ToString();
                }
            }

            //return dm;
            return new DryMatter();
        }

        public AmmoniaRetention GetAmmoniaRetention(int seasonApplicatonId, int dm)
        {

            JArray array = (JArray) rss["agri"]["nmp"]["ammoniaretentions"]["ammoniaretention"];
            var ammoniaRetention = new AmmoniaRetention();

            foreach (var r in array)
            {
                if (Convert.ToInt32(r["seasonapplicatonid"].ToString()) == seasonApplicatonId &&
                    Convert.ToInt32(r["dm"].ToString()) == dm)
                {
                    ammoniaRetention.SeasonApplicationId = Convert.ToInt32(r["seasonapplicatonid"].ToString());
                    ammoniaRetention.DryMatter = Convert.ToInt32(r["dm"].ToString());
                    ammoniaRetention.Value = r["value"].ToString() == "null"
                        ? (decimal?) null
                        : Convert.ToDecimal(r["value"].ToString());
                }
                break;
            }

            return ammoniaRetention;
        }

        public NitrogenMineralization GetNMineralization(int id, int locationid)
        {

            JArray array = (JArray) rss["agri"]["nmp"]["nmineralizations"]["nmineralization"];
            Models.StaticData.NMineralization nmineralization = new Models.StaticData.NMineralization();

            foreach (var r in array)
            {
                if (Convert.ToInt32(r["id"].ToString()) == id &&
                    Convert.ToInt32(r["locationid"].ToString()) == locationid)
                {
                    nmineralization.id = Convert.ToInt32(r["id"].ToString());
                    nmineralization.name = r["name"].ToString();
                    nmineralization.locationid = Convert.ToInt32(r["locationid"].ToString());
                    nmineralization.firstyearvalue = Convert.ToDecimal(r["firstyearvalue"].ToString());
                    nmineralization.longtermvalue = Convert.ToDecimal(r["longtermvalue"].ToString());
                }
            }

            //return nmineralization;
            return new NitrogenMineralization();
        }

        public string GetSoilTestMethod(string id)
        {
            string method = id.ToString();
            JArray array = (JArray) rss["agri"]["nmp"]["soiltestmethods"]["soiltestmethod"];
            JObject rec = array.Children<JObject>().FirstOrDefault(o => o["id"] != null && o["id"].ToString() == id);

            method = rec["name"].ToString();

            return method;
        }

        public List<SoilTestMethod> GetSoilTestMethods()
        {
            var soilTestMethods = new List<SoilTestMethod>();
            JArray array = (JArray)rss["agri"]["nmp"]["soiltestmethods"]["soiltestmethod"];
            foreach (var record in array)
            {
                var soilTestMethod = new SoilTestMethod
                {
                    Id = Convert.ToInt32(record["id"].ToString()),
                    Name = record["name"].ToString(),
                    ConvertToKelownaPHLessThan72 = Convert.ToDecimal(record["ConvertToKelownaPlt72"].ToString()),
                    ConvertToKelownaPHGreaterThanEqual72 = Convert.ToDecimal(record["ConvertToKelownaPge72"].ToString()),
                    ConvertToKelownaK = Convert.ToDecimal(record["ConvertToKelownaK"].ToString()),
                    SortNum = Convert.ToInt16(record["sortNum"].ToString())
                };
                soilTestMethods.Add(soilTestMethod);
            }

            return soilTestMethods;
        }

        public List<SelectListItem> GetSoilTestMethodsDll()
        {
            var soilTestMethods = GetSoilTestMethods();
            List<SelectListItem> soilTestMethodOptions = new List<SelectListItem>();
            foreach (var r in soilTestMethods)
            {
                SelectListItem li = new SelectListItem()
                    { Id = r.Id, Value = r.Name };
                soilTestMethodOptions.Add(li);
            }

            return soilTestMethodOptions;
        }

        public Region GetRegion(int id)
        {

            JArray array = (JArray) rss["agri"]["nmp"]["regions"]["region"];
            Models.StaticData.Region region = new Models.StaticData.Region();

            foreach (var r in array)
            {
                if (Convert.ToInt32(r["id"].ToString()) == id)
                {
                    region.id = Convert.ToInt32(r["id"].ToString());
                    region.name = r["name"].ToString();
                    region.soil_test_phospherous_region_cd =
                        Convert.ToInt32(r["soil_test_phospherous_region_cd"].ToString());
                    region.soil_test_potassium_region_cd =
                        Convert.ToInt32(r["soil_test_potassium_region_cd"].ToString());
                    region.locationid = Convert.ToInt32(r["locationid"].ToString());
                }
            }

            //return region;
            return new Region();
        }

        public PreviousCropType GetPrevCropType(int id)
        {
            Models.StaticData.PrevCropType type = new Models.StaticData.PrevCropType();

            JArray array = (JArray) rss["agri"]["nmp"]["prevcroptypes"]["prevcroptype"];
            JObject rec = array.Children<JObject>()
                .FirstOrDefault(o => o["id"] != null && o["id"].ToString() == id.ToString());

            type.id = Convert.ToInt32(rec["id"].ToString());
            type.prevcropcd = Convert.ToInt32(rec["prevcropcd"].ToString());
            type.name = rec["name"].ToString();
            type.nCreditMetric = Convert.ToInt32(rec["ncreditmetric"].ToString());
            type.nCreditImperial = Convert.ToInt32(rec["ncreditimperial"].ToString());

            //return type;
            return new PreviousCropType();
        }

        public List<PreviousCropType> GetPreviousCropTypes()
        {
            var types = new List<PreviousCropType>();

            JArray array = (JArray) rss["agri"]["nmp"]["prevcroptypes"]["prevcroptype"];

            foreach (var r in array)
            {
                var type = new PreviousCropType
                {
                    Id = Convert.ToInt32(r["id"].ToString()),
                    PreviousCropCode = Convert.ToInt32(r["prevcropcd"].ToString()),
                    Name = r["name"].ToString(),
                    NitrogenCreditMetric = Convert.ToInt32(r["ncreditmetric"].ToString()),
                    NitrogenCreditImperial = Convert.ToInt32(r["ncreditimperial"].ToString())
                };
                types.Add(type);
            }

            return types;
        }

        public List<SelectListItem> GetPrevCropTypesDll(string prevCropCd)
        {
            var types = GetPreviousCropTypes();

            List<SelectListItem> typesOptions = new List<SelectListItem>();

            foreach (var r in types)
            {
                if (r.PreviousCropCode.ToString() == prevCropCd)
                {
                    var li = new SelectListItem()
                        {Id = r.Id, Value = r.Name};
                    typesOptions.Add(li);
                }
            }

            return typesOptions;
        }

        public CropYield GetCropYield(int cropid, int locationid)
        {

            JArray array = (JArray) rss["agri"]["nmp"]["cropyields"]["cropyield"];
            Models.StaticData.CropYield cropYield = new Models.StaticData.CropYield();

            try
            {
                foreach (var r in array)
                {
                    if (Convert.ToInt32(r["cropid"].ToString()) == cropid &&
                        Convert.ToInt32(r["locationid"].ToString()) == locationid)
                    {
                        cropYield.cropid = Convert.ToInt32(r["cropid"].ToString());
                        cropYield.locationid = Convert.ToInt32(r["locationid"].ToString());
                        cropYield.amt = r["amt"].ToString() == "null"
                            ? (decimal?) null
                            : Convert.ToDecimal(r["amt"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
            }

            //return cropYield;
            return new CropYield();
        }

        public SoilTestPhosphorousRecommendation GetSTPRecommend(int stp_kelowna_rangeid,
            int soil_test_phosphorous_region_cd, int phosphorous_crop_group_region_cd)
        {

            JArray array = (JArray) rss["agri"]["nmp"]["stp_recommends"]["stp_recommend"];
            Models.StaticData.STPRecommend sTPRecommend = new Models.StaticData.STPRecommend();

            foreach (var r in array)
            {
                if (Convert.ToInt32(r["stp_kelowna_rangeid"].ToString()) == stp_kelowna_rangeid &&
                    Convert.ToInt32(r["soil_test_phosphorous_region_cd"].ToString()) ==
                    soil_test_phosphorous_region_cd &&
                    Convert.ToInt32(r["phosphorous_crop_group_region_cd"].ToString()) ==
                    phosphorous_crop_group_region_cd)
                {
                    sTPRecommend.stp_kelowna_rangeid = Convert.ToInt32(r["stp_kelowna_rangeid"].ToString());
                    sTPRecommend.soil_test_phosphorous_region_cd =
                        Convert.ToInt32(r["soil_test_phosphorous_region_cd"].ToString());
                    sTPRecommend.phosphorous_crop_group_region_cd =
                        Convert.ToInt32(r["phosphorous_crop_group_region_cd"].ToString());
                    sTPRecommend.p2o5_recommend_kgperha = Convert.ToInt32(r["p2o5_recommend_kgperha"].ToString());
                }
            }

            //return sTPRecommend;
            return new SoilTestPhosphorousRecommendation();
        }

        public SoilTestPhosphorousKelownaRange GetSTPKelownaRangeByPpm(int ppm)
        {

            JArray array = (JArray) rss["agri"]["nmp"]["stp_kelowna_ranges"]["stp_kelowna_range"];
            Models.StaticData.STPKelownaRange sTPKelownaRange = new Models.StaticData.STPKelownaRange();

            foreach (var r in array)
            {
                if (ppm >= Convert.ToInt32(r["range_low"].ToString()) &&
                    ppm <= Convert.ToInt32(r["range_high"].ToString()))
                {
                    sTPKelownaRange.id = Convert.ToInt32(r["id"].ToString());
                    sTPKelownaRange.range = r["range"].ToString();
                    sTPKelownaRange.range_low = Convert.ToInt32(r["range_low"].ToString());
                    sTPKelownaRange.range_high = Convert.ToInt32(r["range_high"].ToString());
                }
            }

            //return sTPKelownaRange;
            return new SoilTestPhosphorousKelownaRange();
        }

        public SoilTestPotassiumRecommendation GetSTKRecommend(int stk_kelowna_rangeid,
            int soil_test_potassium_region_cd, int potassium_crop_group_region_cd)
        {

            JArray array = (JArray) rss["agri"]["nmp"]["stk_recommends"]["stk_recommend"];
            Models.StaticData.STKRecommend sTKRecommend = new Models.StaticData.STKRecommend();

            foreach (var r in array)
            {
                if (Convert.ToInt32(r["stk_kelowna_rangeid"].ToString()) == stk_kelowna_rangeid &&
                    Convert.ToInt32(r["soil_test_potassium_region_cd"].ToString()) == soil_test_potassium_region_cd &&
                    Convert.ToInt32(r["potassium_crop_group_region_cd"].ToString()) == potassium_crop_group_region_cd)
                {
                    sTKRecommend.stk_kelowna_rangeid = Convert.ToInt32(r["stk_kelowna_rangeid"].ToString());
                    sTKRecommend.soil_test_potassium_region_cd =
                        Convert.ToInt32(r["soil_test_potassium_region_cd"].ToString());
                    sTKRecommend.potassium_crop_group_region_cd =
                        Convert.ToInt32(r["potassium_crop_group_region_cd"].ToString());
                    sTKRecommend.k2o_recommend_kgperha = Convert.ToInt32(r["k2o_recommend_kgperha"].ToString());
                }
            }

            //return sTKRecommend;
            return new SoilTestPotassiumRecommendation();
        }

        public SoilTestPotassiumKelownaRange GetSTKKelownaRangeByPpm(int ppm)
        {

            JArray array = (JArray) rss["agri"]["nmp"]["stk_kelowna_ranges"]["stk_kelowna_range"];
            Models.StaticData.STKKelownaRange sTKKelownaRange = new Models.StaticData.STKKelownaRange();

            foreach (var r in array)
            {
                if (ppm >= Convert.ToInt32(r["range_low"].ToString()) &&
                    ppm <= Convert.ToInt32(r["range_high"].ToString()))
                {
                    sTKKelownaRange.id = Convert.ToInt32(r["id"].ToString());
                    sTKKelownaRange.range = r["range"].ToString();
                    sTKKelownaRange.range_low = Convert.ToInt32(r["range_low"].ToString());
                    sTKKelownaRange.range_high = Convert.ToInt32(r["range_high"].ToString());
                }
            }

            //return sTKKelownaRange;
            return new SoilTestPotassiumKelownaRange();
        }

        public ConversionFactor GetConversionFactor()
        {
            return new ConversionFactor
            {
                NitrogenProteinConversion =
                    Convert.ToDecimal((string) rss["agri"]["nmp"]["conversions"]["n_protein_conversion"]),
                UnitConversion = Convert.ToDecimal((string) rss["agri"]["nmp"]["conversions"]["unit_conversion"]),
                DefaultSoilTestKelownaPhosphorous =
                    Convert.ToInt16((string) rss["agri"]["nmp"]["conversions"]["defaultSoilTestKelownaP"]),
                DefaultSoilTestKelownaPotassium =
                    Convert.ToInt16((string) rss["agri"]["nmp"]["conversions"]["defaultSoilTestKelownaK"]),
                KilogramPerHectareToPoundPerAcreConversion =
                    Convert.ToDecimal((string) rss["agri"]["nmp"]["conversions"]["kgperha_lbperac_conversion"]),
                PotassiumAvailabilityFirstYear =
                    Convert.ToDecimal((string) rss["agri"]["nmp"]["conversions"]["potassiumAvailabilityFirstYear"]),
                PotassiumAvailabilityLongTerm =
                    Convert.ToDecimal((string) rss["agri"]["nmp"]["conversions"]["potassiumAvailabilityLongTerm"]),
                PotassiumKtoK2OConversion =
                    Convert.ToDecimal((string) rss["agri"]["nmp"]["conversions"]["potassiumKtoK2Oconversion"]),
                PhosphorousAvailabilityFirstYear =
                    Convert.ToDecimal((string) rss["agri"]["nmp"]["conversions"]["phosphorousAvailabilityFirstYear"]),
                PhosphorousAvailabilityLongTerm =
                    Convert.ToDecimal((string) rss["agri"]["nmp"]["conversions"]["phosphorousAvailabilityLongTerm"]),
                PhosphorousPtoP2O5Conversion =
                    Convert.ToDecimal((string) rss["agri"]["nmp"]["conversions"]["phosphorousPtoP2O5Kconversion"]),
                PoundPerTonConversion =
                    Convert.ToDecimal((string) rss["agri"]["nmp"]["conversions"]["lbPerTonConversion"]),
                PoundPer1000FtSquaredToPoundPerAcreConversion =
                    Convert.ToDecimal(
                        (string) rss["agri"]["nmp"]["conversions"]["lbper1000ftsquared_lbperac_conversion"]),
                DefaultApplicationOfManureInPrevYears =
                    (rss["agri"]["nmp"]["conversions"]["defaultApplicationOfManureInPrevYears"]).ToString(),
                SoilTestPPMToPoundPerAcre =
                    Convert.ToDecimal((string)rss["agri"]["nmp"]["soilTestPPMToPoundPerAcreConversionFactor"]["ppmToPoundPerAcre"]),
            };
        }

        public BalanceMessages GetMessageByChemicalBalance(string balanceType, long balance, bool legume)
        {
            JArray array = (JArray) rss["agri"]["nmp"]["messages"]["message"];
            BalanceMessages bm = new BalanceMessages();

            foreach (var r in array)
            {
                if (balanceType == r["balanceType"].ToString() &&
                    balance >= Convert.ToInt32(r["balance_low"].ToString()) &&
                    balance <= Convert.ToInt32(r["balance_high"].ToString()))
                {
                    bm.Chemical = balanceType;
                    if (r["displayMessage"].ToString() == "Yes")
                        bm.Message = string.Format(r["text"].ToString(), Math.Abs(balance).ToString());
                    bm.Icon = r["icon"].ToString();

                    if (balanceType == "AgrN" && legume && bm.Icon != "stop")
                    {
                        // nitrogen does not need to be added even if there is a deficiency
                        bm.Icon = "good";
                        bm.Message = "";
                    }

                    bm.IconText = GetNutrientIcon(bm.Icon).Definition;
                    return bm;
                }
            }

            return bm;
        }

        public string GetMessageByChemicalBalance(string balanceType, long balance, bool legume, decimal soilTest)
        {
            JArray array = (JArray) rss["agri"]["nmp"]["messages"]["message"];
            string message = null;

            foreach (var r in array)
            {
                if (balanceType == r["balanceType"].ToString() &&
                    balance >= Convert.ToInt32(r["balance_low"].ToString()) &&
                    balance <= Convert.ToInt32(r["balance_high"].ToString()) &&
                    soilTest >= Convert.ToDecimal(r["soiltest_low"].ToString()) &&
                    soilTest <= Convert.ToDecimal(r["soiltest_high"].ToString()))
                {
                    message = string.Format(r["text"].ToString(), Math.Abs(balance).ToString());
                }

                //If legume crop in field never display that more N is required
                if (balanceType == "AgrN" &&
                    legume &&
                    Convert.ToInt32(r["balance_high"].ToString()) == 9999)
                {
                    message = string.Empty;
                }
            }

            return message;
        }

        public BalanceMessages GetMessageByChemicalBalance(string balanceType, long balance1, long balance2,
            string assignedChemical)
        {
            JArray array = (JArray) rss["agri"]["nmp"]["messages"]["message"];
            BalanceMessages bm = new BalanceMessages();

            foreach (var r in array)
            {
                if (balanceType == r["balanceType"].ToString() &&
                    balance1 >= Convert.ToInt32(r["balance_low"].ToString()) &&
                    balance1 <= Convert.ToInt32(r["balance_high"].ToString()) &&
                    balance2 >= Convert.ToInt32(r["balance1_low"].ToString()) &&
                    balance2 <= Convert.ToInt32(r["balance1_high"].ToString()))
                {
                    bm.Chemical = assignedChemical;
                    if (r["displayMessage"].ToString() == "Yes")
                        bm.Message = r["text"].ToString();
                    bm.Icon = r["icon"].ToString();
                }
            }

            return bm;
        }

        public FertilizerType GetFertilizerType(string id)
        {
            string x = id.ToString();
            JArray array = (JArray) rss["agri"]["nmp"]["fertilizertypes"]["fertilizertype"];
            JObject rec = array.Children<JObject>().FirstOrDefault(o => o["id"] != null && o["id"].ToString() == id);

            Models.StaticData.FertilizerType type = new Models.StaticData.FertilizerType();
            type.id = Convert.ToInt32(rec["id"].ToString());
            type.name = rec["name"].ToString();
            type.dry_liquid = rec["dry_liquid"].ToString();
            type.custom = rec["customfertilizer"].ToString() == "true" ? true : false;

            //return type;
            return new FertilizerType();
        }

        public List<FertilizerType> GetFertilizerTypes()
        {
            var types = new List<FertilizerType>();
            JArray array = (JArray) rss["agri"]["nmp"]["fertilizertypes"]["fertilizertype"];

            foreach (var r in array)
            {
                var type = new FertilizerType
                {
                    Id = Convert.ToInt32(r["id"].ToString()),
                    Name = r["name"].ToString(),
                    DryLiquid = r["dry_liquid"].ToString(),
                    Custom = r["customfertilizer"].ToString() == "true"
                };
                types.Add(type);
            }

            return types;
        }

        public List<SelectListItem> GetFertilizerTypesDll()
        {
            var types = GetFertilizerTypes();

            List<SelectListItem> typesOptions = new List<SelectListItem>();

            foreach (var r in types)
            {
                var li = new SelectListItem()
                {
                    Id = r.Id, Value = r.Name
                };
                typesOptions.Add(li);
            }

            return typesOptions;
        }

        public Fertilizer GetFertilizer(string id)
        {
            string x = id.ToString();
            JArray array = (JArray) rss["agri"]["nmp"]["fertilizers"]["fertilizer"];
            JObject rec = array.Children<JObject>().FirstOrDefault(o => o["id"] != null && o["id"].ToString() == id);

            Models.StaticData.Fertilizer fertilizer = new Models.StaticData.Fertilizer();
            fertilizer.id = Convert.ToInt32(rec["id"].ToString());
            fertilizer.name = rec["name"].ToString();
            fertilizer.dry_liquid = rec["dry_liquid"].ToString();
            fertilizer.nitrogen = Convert.ToDecimal(rec["nitrogen"].ToString());
            fertilizer.phosphorous = Convert.ToDecimal(rec["phosphorous"].ToString());
            fertilizer.potassium = Convert.ToDecimal(rec["potassium"].ToString());
            fertilizer.sortNum = Convert.ToInt32(rec["sortNum"].ToString());

            //return fertilizer;
            return new Fertilizer();
        }

        public List<Fertilizer> GetFertilizers()
        {
            var fertilizers = new List<Fertilizer>();
            JArray array = (JArray) rss["agri"]["nmp"]["fertilizers"]["fertilizer"];

            foreach (var r in array)
            {

                var fertilizer = new Fertilizer
                {
                    Id = Convert.ToInt32(r["id"].ToString()),
                    Name = r["name"].ToString(),
                    DryLiquid = r["dry_liquid"].ToString(),
                    Nitrogen = Convert.ToDecimal(r["nitrogen"].ToString()),
                    Phosphorous = Convert.ToDecimal(r["phosphorous"].ToString()),
                    Potassium = Convert.ToDecimal(r["potassium"].ToString()),
                    SortNum = Convert.ToInt32(r["sortNum"].ToString())
                };

                fertilizers.Add(fertilizer);
            }

            return fertilizers;
        }

        public List<SelectListItem> GetFertilizersDll(string fertilizerType)
        {
            var types = GetFertilizers();

            types = types.OrderBy(n => n.SortNum).ThenBy(n => n.Name).ToList();

            List<SelectListItem> typesOptions = new List<SelectListItem>();

            foreach (var r in types)
            {
                if (r.DryLiquid.ToString() == fertilizerType)
                {
                    var li = new SelectListItem()
                        {Id = r.Id, Value = r.Name};
                    typesOptions.Add(li);
                }
            }

            return typesOptions;
        }

        public SoilTestMethod GetSoilTestMethodByMethod(string _soilTest)
        {
            JArray items = (JArray) rss["agri"]["nmp"]["soiltestmethods"]["soiltestmethod"];
            Models.StaticData.SoilTestMethod soilTestMethod = new Models.StaticData.SoilTestMethod();

            foreach (var r in items)
            {
                if (_soilTest == r["name"].ToString())
                {
                    soilTestMethod.id = Convert.ToInt32(r["id"].ToString());
                    soilTestMethod.name = r["name"].ToString();
                    soilTestMethod.ConvertToKelownaPlt72 = Convert.ToDecimal(r["ConvertToKelownaPlt72"].ToString());
                    soilTestMethod.ConvertToKelownaPge72 = Convert.ToDecimal(r["ConvertToKelownaPge72"].ToString());
                    soilTestMethod.ConvertToKelownaK = Convert.ToDecimal(r["ConvertToKelownaK"].ToString());
                }
            }

            //return soilTestMethod;
            return new SoilTestMethod();
        }

        public SoilTestMethod GetSoilTestMethodById(string id)
        {
            JArray items = (JArray) rss["agri"]["nmp"]["soiltestmethods"]["soiltestmethod"];
            Models.StaticData.SoilTestMethod soilTestMethod = new Models.StaticData.SoilTestMethod();
            int idParsed = 0;
            int.TryParse(id, out idParsed);

            try
            {
                foreach (var r in items)
                {
                    if (idParsed == Convert.ToInt16(r["id"].ToString()))
                    {
                        soilTestMethod.id = Convert.ToInt32(r["id"].ToString());
                        soilTestMethod.name = r["name"].ToString();
                        soilTestMethod.ConvertToKelownaPlt72 = Convert.ToDecimal(r["ConvertToKelownaPlt72"].ToString());
                        soilTestMethod.ConvertToKelownaPge72 = Convert.ToDecimal(r["ConvertToKelownaPge72"].ToString());
                        soilTestMethod.ConvertToKelownaK = Convert.ToDecimal(r["ConvertToKelownaK"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle error
            }

            //return soilTestMethod;
            return new SoilTestMethod();
        }

        public LiquidFertilizerDensity GetLiquidFertilizerDensity(int fertilizerId, int densityId)
        {
            JArray array = (JArray) rss["agri"]["nmp"]["liquidfertilizerdensitys"]["liquidfertilizerdensity"];
            JObject rec = array.Children<JObject>().FirstOrDefault(o =>
                o["fertilizerid"] != null && o["fertilizerid"].ToString() == fertilizerId.ToString() &&
                o["densityunitid"] != null && o["densityunitid"].ToString() == densityId.ToString());

            Models.StaticData.LiquidFertilizerDensity density = new Models.StaticData.LiquidFertilizerDensity();
            density.id = Convert.ToInt32(rec["id"].ToString());
            density.value = Convert.ToDecimal(rec["value"].ToString());

            //return density;
            return new LiquidFertilizerDensity();
        }

        public DefaultSoilTest GetDefaultSoilTest()
        {
            Models.StaticData.DefaultSoilTest dt = new Models.StaticData.DefaultSoilTest();

            dt.nitrogen = Convert.ToDecimal((string) rss["agri"]["nmp"]["conversions"]["defaultSoilTestNitrogen"]);
            dt.phosphorous = Convert.ToDecimal((string) rss["agri"]["nmp"]["conversions"]["defaultSoilTestKelownaP"]);
            dt.potassium = Convert.ToDecimal((string) rss["agri"]["nmp"]["conversions"]["defaultSoilTestKelownaK"]);
            dt.pH = Convert.ToDecimal((string) rss["agri"]["nmp"]["conversions"]["defaultSoilTestpH"]);
            dt.convertedKelownaP =
                Convert.ToInt32((string) rss["agri"]["nmp"]["conversions"]["defaultSoilTestKelownaP"]);
            dt.convertedKelownaK =
                Convert.ToInt32((string) rss["agri"]["nmp"]["conversions"]["defaultSoilTestKelownaK"]);

            //return dt;
            return new DefaultSoilTest();
        }

        public string GetDefaultSoilTestMethod()
        {
            Models.StaticData.DefaultSoilTest dt = new Models.StaticData.DefaultSoilTest();

            return (string) rss["agri"]["nmp"]["conversions"]["defaultSoilTestMethodId"];
        }

        public string SoilTestRating(string chem, decimal value)
        {
            string results = "Ukn";

            Models.StaticData.Fertilizers fertilizers = new Models.StaticData.Fertilizers();
            List<Models.StaticData.SoilTestRange> ranges = new List<Models.StaticData.SoilTestRange>();

            JArray array = (JArray) rss["agri"]["nmp"]["soiltestranges"][chem];

            foreach (var r in array)
            {
                Models.StaticData.SoilTestRange range = new Models.StaticData.SoilTestRange();
                range.upperLimit = Convert.ToInt32(r["upperlimit"].ToString());
                range.rating = r["rating"].ToString();
                ranges.Add(range);
            }

            for (int i = 0; i < ranges.Count(); i++)
            {
                if (value < ranges[i].upperLimit)
                {
                    results = ranges[i].rating;
                    break;
                }
            }

            return results;
        }


        public FertilizerMethod GetFertilizerMethod(string id)
        {
            JArray array = (JArray) rss["agri"]["nmp"]["fertilizermethods"]["fertilizermethod"];
            JObject rec = array.Children<JObject>().FirstOrDefault(o => o["id"] != null && o["id"].ToString() == id);

            Models.StaticData.FertilizerMethod fertilizerMethod = new Models.StaticData.FertilizerMethod();
            fertilizerMethod.id = Convert.ToInt32((string) rec["id"]);
            fertilizerMethod.name = (string) rec["name"];

            //return fertilizerMethod;
            return new FertilizerMethod();
        }

        public List<FertilizerMethod> GetFertilizerMethods()
        {
            var feritilizerMethods = new List<FertilizerMethod>();

            JArray array = (JArray)rss["agri"]["nmp"]["fertilizermethods"]["fertilizermethod"];
            foreach (var record in array)
            {
                var feritilizerMethod = new FertilizerMethod()
                {
                    Id = Convert.ToInt32(record["id"].ToString()),
                    Name = record["name"].ToString()
                };
                feritilizerMethods.Add(feritilizerMethod);
            }

            return feritilizerMethods;
        }

        public List<SelectListItem> GetFertilizerMethodsDll()
        {
            var fertilizerMethods = GetFertilizerMethods();
            List<SelectListItem> fertilizerMethodOptions = new List<SelectListItem>();
            foreach (var r in fertilizerMethods)
            {
                SelectListItem li = new SelectListItem()
                { Id = r.Id, Value = r.Name };
                fertilizerMethodOptions.Add(li);
            }

            return fertilizerMethodOptions;

        }

        public string GetSoilTestWarning()
        {
            string template = GetUserPrompt("defaultsoiltest");
            decimal pH = Convert.ToDecimal((string) rss["agri"]["nmp"]["conversions"]["defaultSoilTestpH"]);
            decimal phosphorous =
                Convert.ToDecimal((string) rss["agri"]["nmp"]["conversions"]["defaultSoilTestKelownaP"]);
            decimal potassium =
                Convert.ToDecimal((string) rss["agri"]["nmp"]["conversions"]["defaultSoilTestKelownaK"]);

            string msg = string.Format(template, phosphorous, potassium, pH);

            return msg;
        }

        public string GetExternalLink(string name)
        {
            string result = string.Empty;

            JArray array = (JArray) rss["agri"]["nmp"]["externallinks"]["externallink"];
            JObject rec = array.Children<JObject>().FirstOrDefault(o => o["name"].ToString() == name);

            if (rec != null)
                result = (string) rec["url"];

            return result;
        }

        public string GetUserPrompt(string name)
        {
            string result = string.Empty;

            JArray array = (JArray) rss["agri"]["nmp"]["userprompts"]["userprompt"];
            JObject rec = array.Children<JObject>().FirstOrDefault(o => o["name"].ToString() == name);

            if (rec != null)
                result = (string) rec["text"];

            return result;
        }

        public Version GetVersionData()
        {
            var version = new Version();

            version.StaticDataVersion = (string) rss["agri"]["nmp"]["version"]["staticDataVersion"];

            return version;
        }

        public string GetStaticDataVersion()
        {
            string result = string.Empty;

            result = rss["agri"]["nmp"]["versions"]["staticDataVersion"].ToString();

            return result;
        }

        //public List<StaticDataValidationMessages> ValidateRelationship(string childNode, string childfield,
        //    string parentNode, string parentfield)
        //{
        //    List<StaticDataValidationMessages> messages = new List<StaticDataValidationMessages>();

        //    JArray childArray = (JArray) rss.SelectToken(childNode);
        //    JArray parentArray = (JArray) rss.SelectToken(parentNode);

        //    string matchP = string.Empty;
        //    string matchC = string.Empty;
        //    bool relationshipOK = false;

        //    // iterate over children
        //    foreach (var c in childArray)
        //    {
        //        relationshipOK = false;
        //        //get the child relationship field
        //        matchC = c.SelectToken(childfield).ToString();

        //        //look for matching parent
        //        foreach (var p in parentArray)
        //        {
        //            matchP = p.SelectToken(parentfield).ToString();
        //            //if (rel == c.SelectToken(childfield).ToString())
        //            if (matchP == matchC || matchC == "null")
        //                relationshipOK = true;
        //        }

        //        if (!relationshipOK)
        //        {
        //            StaticDataValidationMessages message = new StaticDataValidationMessages();
        //            message.Child = childNode;
        //            message.LinkData = matchC;
        //            message.Parent = parentNode;
        //            messages.Add(message);
        //            message = null;
        //        }
        //    }

        //    return messages;
        //}
        
        public List<PreviousManureApplicationYear> GetPrevManureApplicationInPrevYears()
        {
            var selections = new List<PreviousManureApplicationYear>();
            var jsonPrevYearManure = (JArray)rss["agri"]["nmp"]["manureprevyearscd"]["manureprevyearcd"];

            foreach (var r in jsonPrevYearManure)
            {
                var sel = new PreviousManureApplicationYear
                {
                    FieldManureApplicationHistory = Convert.ToInt32(r["id"].ToString()),
                    Name = r["name"].ToString()
                };
                selections.Add(sel);
            }

            return selections;
        }

        public List<PreviousYearManureApplicationNitrogenDefault> GetPrevYearManureNitrogenCreditDefaults()
        {
            var nitrogrenDefaults = (JArray) rss["agri"]["nmp"]["defaultprevyearmanureapplfrequency"]["defprevyearmanurenitrogen"];
            var result = new List<PreviousYearManureApplicationNitrogenDefault>();

            foreach (var r in nitrogrenDefaults)
            {
                //var defaultNitrogen = JsonConvert.DeserializeObject<PreviousYearManureApplicationNitrogenDefault>(r.ToString());
                var defaultNitrogen = new PreviousYearManureApplicationNitrogenDefault()
                {
                    FieldManureApplicationHistory = Convert.ToInt32(r["prevYearManureAppFrequency"].ToString()),
                    DefaultNitrogenCredit = JsonConvert.DeserializeObject<int[]>(r["defaultNitrogenCredit"].ToString()),
                };

                result.Add(defaultNitrogen);
            }

            return result;
        }

        public bool wasManureAddedInPreviousYear(string userSelectedPrevYearsManureAdded)
        {
            string noManureFromPreviousYearsCd =
                (string) rss["agri"]["nmp"]["manureprevyearscd"]["manureprevyearcd"][0]["id"];
            // assumes first element (id=0) denotes no manure added in previous years.
            return (userSelectedPrevYearsManureAdded != noManureFromPreviousYearsCd);
        }

        public int GetInteriorId()
        {
            string interiorId =
                (string) rss["agri"]["nmp"]["locations"]["location"][0]["id"]; // assumes first element is interior
            return Convert.ToInt32(interiorId);
        }

        public bool IsRegionInteriorBC(int? region)
        {
            var regions = GetRegions();
            if (region != null)
            {
                foreach (var r in regions)
                {
                    if (r.Id == region)
                        if (r.LocationId == GetInteriorId())
                            return true;
                        else
                            return false;
                }
            }

            return false;
        }

        private DateTime GetInteriorNitrateSampleFromDt(int yearOfAnalysis)
        {
            string fromDtMonth =
                (string) rss["agri"]["nmp"]["interiorBCSampleDtForNitrateCredit"][
                    "fromDateMonth"]; // assumes first element is interior
            return new DateTime(yearOfAnalysis - 1, Convert.ToInt16(fromDtMonth), 01);
        }

        private DateTime GetInteriorNitrateSampleToDt(int yearOfAnalysis)
        {
            string toDtMonth = (string) rss["agri"]["nmp"]["interiorBCSampleDtForNitrateCredit"]["toDateMonth"];
            return new DateTime(yearOfAnalysis, Convert.ToInt16(toDtMonth),
                DateTime.DaysInMonth(yearOfAnalysis, Convert.ToInt16(toDtMonth)));
        }

        private DateTime GetCoastalNitrateSampleFromDt(int yearOfAnalysis)
        {
            string fromDtMonth =
                (string) rss["agri"]["nmp"]["coastalBCSampleDtForNitrateCredit"][
                    "fromDateMonth"]; // assumes first element is interior
            return new DateTime(yearOfAnalysis, Convert.ToInt16(fromDtMonth), 01);
        }

        private DateTime GetCoastalNitrateSampleToDt(int yearOfAnalysis)
        {
            string toDtMonth = (string) rss["agri"]["nmp"]["coastalBCSampleDtForNitrateCredit"]["toDateMonth"];
            return new DateTime(yearOfAnalysis, Convert.ToInt16(toDtMonth),
                DateTime.DaysInMonth(yearOfAnalysis, Convert.ToInt16(toDtMonth)));
        }

        public bool IsNitrateCreditApplicable(int? region, DateTime sampleDate, int yearOfAnalysis)
        {
            if ((region != null) && (sampleDate != null))
            {
                if (IsRegionInteriorBC(region))
                    return ((sampleDate >= GetInteriorNitrateSampleFromDt(yearOfAnalysis)) &&
                            (sampleDate <= GetInteriorNitrateSampleToDt(yearOfAnalysis)));
                else // coastal farm
                    return ((sampleDate >= GetCoastalNitrateSampleFromDt(yearOfAnalysis)) &&
                            (sampleDate <= GetCoastalNitrateSampleToDt(yearOfAnalysis)));
            }

            return false;
        }

        public decimal GetSoilTestNitratePPMToPoundPerAcreConversionFactor()
        {
            string conversionFactor =
                (string) rss["agri"]["nmp"]["soilTestPPMToPoundPerAcreConversionFactor"]["ppmToPoundPerAcre"];
            return Convert.ToDecimal(conversionFactor);
        }

        public List<Browser> GetAllowableBrowsers()
        {
            var browsers = new List<Browser>();

            JArray array = (JArray) rss["agri"]["nmp"]["browsers"]["browser"];

            foreach (var r in array)
            {
                var browser = new Browser()
                {
                    Id = Convert.ToInt32(r["id"].ToString()),
                    Name = r["name"].ToString(),
                    MinVersion = r["minversion"].ToString()
                };
                browsers.Add(browser);
            }

            return browsers;
        }

        public bool IsManureClassCompostType(string manure_class)
        {
            return (manure_class == MANURE_CLASS_COMPOST);
        }

        public bool IsManureClassCompostClassType(string manure_class)
        {
            return (manure_class == MANURE_CLASS_COMPOST_BOOK);
        }

        public bool IsManureClassOtherType(string manure_class)
        {
            return (manure_class == MANURE_CLASS_OTHER);
        }

        private string ParseStdUnit(string stdUnit)
        {
            int idx = stdUnit.LastIndexOf("/");
            if (idx > 0)
                stdUnit = stdUnit.Substring(0, idx);

            return stdUnit;
        }

        public string GetManureRptStdUnit(string solidLiquid)
        {
            string stdUnit;

            if (solidLiquid.ToUpper() == "SOLID")
                stdUnit = GetUnit((string) rss["agri"]["nmp"]["RptCompletedManureRequired_StdUnit"]["solid_unit_id"])
                    .Name;
            else
                stdUnit = GetUnit((string) rss["agri"]["nmp"]["RptCompletedManureRequired_StdUnit"]["liquid_unit_id"])
                    .Name;

            return (ParseStdUnit(stdUnit));
        }

        public string GetFertilizerRptStdUnit(string dryLiquid)
        {
            string stdUnit;

            if (dryLiquid.ToUpper() == "DRY")
                stdUnit = GetFertilizerUnit(Convert.ToInt16(
                    rss["agri"]["nmp"]["RptCompletedFertilizerRequired_StdUnit"]["solid_unit_id"].ToString())).Name;
            else
                stdUnit = GetFertilizerUnit(Convert.ToInt16(
                    rss["agri"]["nmp"]["RptCompletedFertilizerRequired_StdUnit"]["liquid_unit_id"].ToString())).Name;

            return (ParseStdUnit(stdUnit));
        }

        public bool IsCustomFertilizer(int fertilizerTypeID)
        {
            JArray fertTypes = (JArray) rss["agri"]["nmp"]["fertilizertypes"]["fertilizertype"];
            foreach (var r in fertTypes)
            {
                if (Convert.ToInt16(r["id"].ToString()) == fertilizerTypeID)
                    return Convert.ToBoolean(r["customfertilizer"].ToString());
            }

            return false;
        }

        public bool IsFertilizerTypeDry(int fertilizerTypeID)
        {
            JArray fertTypes = (JArray) rss["agri"]["nmp"]["fertilizertypes"]["fertilizertype"];
            foreach (var r in fertTypes)
            {
                if (Convert.ToInt16(r["id"].ToString()) == fertilizerTypeID)
                    return (r["dry_liquid"].ToString().ToUpper() == "DRY");
            }

            return false;
        }

        public bool IsFertilizerTypeLiquid(int fertilizerTypeID)
        {
            JArray fertTypes = (JArray) rss["agri"]["nmp"]["fertilizertypes"]["fertilizertype"];
            foreach (var r in fertTypes)
            {
                if (Convert.ToInt16(r["id"].ToString()) == fertilizerTypeID)
                    return (r["dry_liquid"].ToString().ToUpper() == "LIQUID");
            }

            return false;
        }

        public FertilizerType GetFertilizerType(int fertilizerTypeID)
        {
            JArray fertTypes = (JArray) rss["agri"]["nmp"]["fertilizertypes"]["fertilizertype"];

            foreach (var r in fertTypes)
            {
                if (Convert.ToInt16(r["id"].ToString()) == fertilizerTypeID)
                {
                    return new FertilizerType()
                    {
                        Id = Convert.ToInt16(r["id"].ToString()),
                        Custom = Convert.ToBoolean(r["customfertilizer"].ToString()),
                        DryLiquid = r["dry_liquid"].ToString(),
                        Name = r["name"].ToString()
                    };
                }
            }

            return null;
        }

        public bool IsCropGrainsAndOilseeds(int cropType)
        {
            return (cropType == CROPTYPE_GRAINS_OILSEEDS_ID);
        }

        public List<SelectListItem> GetCropHarvestUnitsDll()
        {
            JArray fertTypes = (JArray) rss["agri"]["nmp"]["harvestunits"]["harvestunit"];

            List<SelectListItem> harvestUnitsOptions = new List<SelectListItem>();
            foreach (var r in fertTypes)
            {
                var li = new SelectListItem()
                    {Id = Convert.ToInt16(r["id"].ToString()), Value = r["name"].ToString()};
                harvestUnitsOptions.Add(li);
            }

            return harvestUnitsOptions;
        }


        public string GetHarvestYieldUnitName(string yieldUnit)
        {
            JArray fertTypes = (JArray) rss["agri"]["nmp"]["harvestunits"]["harvestunit"];

            List<SelectListItem> harvestUnitsOptions = new List<SelectListItem>();
            foreach (var r in fertTypes)
            {
                if (r["id"].ToString() == yieldUnit)
                    return r["name"].ToString();
            }

            return "unit measure not found";
        }

        public string GetHarvestYieldDefaultUnitName()
        {
            return GetHarvestYieldUnitName(CROP_YIELD_DEFAULT_CALCULATION_UNIT.ToString());
        }

        public bool IsCropHarvestYieldDefaultUnit(int selectedCropYieldUnit)
        {
            return (selectedCropYieldUnit == CROP_YIELD_DEFAULT_CALCULATION_UNIT);
        }

        public int GetHarvestYieldDefaultUnit()
        {
            return CROP_YIELD_DEFAULT_CALCULATION_UNIT;
        }

        public int GetHarvestYieldDefaultDisplayUnit()
        {
            return CROP_YIELD_DEFAULT_DISPLAY_UNIT;
        }

        public decimal ConvertYieldFromBushelToTonsPerAcre(int cropid, decimal yield)
        {
            Crop crop = GetCrop(cropid);
            if (crop.HarvestBushelsPerTon.HasValue)
                return (yield / Convert.ToDecimal(crop.HarvestBushelsPerTon));
            return -1;
        }

        public List<NutrientIcon> GetNutrientIcons()
        {
            var icons = new List<NutrientIcon>();

            JArray array = (JArray) rss["agri"]["nmp"]["nutrienticons"]["nutrienticon"];

            foreach (var r in array)
            {
                var icon = new NutrientIcon
                {
                    Id = Convert.ToInt32(r["id"].ToString()),
                    Name = r["name"].ToString(),
                    Definition = r["definition"].ToString()
                };
                icons.Add(icon);
            }

            return icons;
        }

        public NutrientIcon GetNutrientIcon(string name)
        {
            Models.StaticData.NutrientIcon icon = new Models.StaticData.NutrientIcon();

            JArray icons = (JArray) rss["agri"]["nmp"]["nutrienticons"]["nutrienticon"];

            foreach (var r in icons)
            {
                if (r["name"].ToString() == name)
                {
                    icon.id = Convert.ToInt32(r["id"].ToString());
                    icon.name = r["name"].ToString();
                    icon.definition = r["definition"].ToString();
                }
            }

            //return icon;
            return new NutrientIcon();
        }

        public List<Animal> GetAnimals()
        {
            var animals = new List<Animal>();

            JArray array = (JArray)rss["agri"]["nmp"]["animals"]["animal"];
            foreach (var record in array)
            {
                var animal = new Animal
                {
                    Id = Convert.ToInt32(record["id"].ToString()),
                    Name = record["Name"].ToString()
                };
                animals.Add(animal);
            }

            return animals;
        }

        public Animal GetAnimal(int id)
        {
            JArray array = (JArray)rss["agri"]["nmp"]["animals"]["animal"];
            JObject rec = array.Children<JObject>()
                .FirstOrDefault(o => o["id"] != null && o["id"].ToString() == id.ToString());
            var animal = new Animal
            {
                Id = Convert.ToInt32(rec["id"].ToString()),
                Name = rec["Name"].ToString()
            };

            return animal;
        }

        public List<SelectListItem> GetAnimalTypesDll()
        {
            var animalTypes = GetAnimals();

            List<SelectListItem> animalTypeOptions = new List<SelectListItem>();

            foreach (var r in animalTypes)
            {
                var li = new SelectListItem()
                    { Id = r.Id, Value = r.Name };
                animalTypeOptions.Add(li);
            }

            return animalTypeOptions;
        }

        public List<AnimalSubType> GetAnimalSubTypes(int animalId)
        {
            var subTypes = new List<AnimalSubType>();

            JArray array = (JArray) rss["agri"]["nmp"]["animalSubTypes"]["animalSubType"];
            foreach (var record in array)
            {
                if (Convert.ToUInt32(record["animalId"].ToString()) == animalId)
                {
                    var animalSubtype = new AnimalSubType
                    {
                        Id = Convert.ToInt32(record["id"]),
                        Name = record["name"].ToString(),
                        LiquidPerGalPerAnimalPerDay = Convert.ToDecimal(record["liquidPerGalPerAnimalPerDay"]),
                        SolidPerGalPerAnimalPerDay = Convert.ToDecimal(record["solidPerGalPerAnimalPerDay"]),
                        SolidPerPoundPerAnimalPerDay = Convert.ToDecimal(record["solidPerPoundPerAnimalPerDay"]),
                        SolidLiquidSeparationPercentage = 
                            Convert.ToDecimal(record["solidLiquidSeparationPercentage"]),
                        AnimalId = Convert.ToInt32(record["animalId"])
                    };
                    subTypes.Add(animalSubtype);
                }
            }

            return subTypes;
        }

        public List<AnimalSubType> GetAnimalSubTypes()
        {
            var animalSubTypes = new List<AnimalSubType>();

            JArray array = (JArray)rss["agri"]["nmp"]["animalSubTypes"]["animalSubType"];
            foreach (var record in array)
            {
                var animalSubtype = new AnimalSubType
                {
                    Id = Convert.ToInt32(record["id"].ToString()),
                    Name = record["name"].ToString(),
                    LiquidPerGalPerAnimalPerDay = !string.IsNullOrWhiteSpace(record["liquidPerGalPerAnimalPerDay"].ToString()) ? 
                                                Convert.ToDecimal(record["liquidPerGalPerAnimalPerDay"].ToString()) : 0,
                    SolidPerGalPerAnimalPerDay = !string.IsNullOrWhiteSpace(record["solidPerGalPerAnimalPerDay"].ToString()) ? 
                                                Convert.ToDecimal(record["solidPerGalPerAnimalPerDay"].ToString()) : 0,
                    SolidPerPoundPerAnimalPerDay = !string.IsNullOrWhiteSpace(record["solidPerPoundPerAnimalPerDay"].ToString()) ?
                                                Convert.ToDecimal(record["solidPerPoundPerAnimalPerDay"].ToString()) : 0,
                    SolidLiquidSeparationPercentage = !string.IsNullOrWhiteSpace(record["solidLiquidSeparationPercentage"].ToString()) ?
                                                 Convert.ToDecimal(record["solidLiquidSeparationPercentage"].ToString()) : 0,
                    AnimalId = Convert.ToInt32(record["animalId"].ToString())
                };
                animalSubTypes.Add(animalSubtype);
            }

            return animalSubTypes;
        }

        public List<SelectListItem> GetSubtypesDll(int animalType)
        {
            var animalSubTypes = GetAnimalSubTypes();

            animalSubTypes = animalSubTypes.OrderBy(n => n.Name).ToList();

            List<SelectListItem> animalSubTypesOptions = new List<SelectListItem>();

            foreach (var r in animalSubTypes)
            {
                if (r.AnimalId == animalType)
                {
                    var li = new SelectListItem()
                        { Id = r.Id, Value = r.Name };
                    animalSubTypesOptions.Add(li);
                }
            }

            return animalSubTypesOptions;
        }

        //public List<ManureMaterialType> GetManureMaterialTypes()
        //{
        //    var manureMaterialTypes = new List<ManureMaterialType>();
        //    JArray array = (JArray)rss["agri"]["nmp"]["manureMaterialTypes"]["manureMaterialType"];

        //    foreach (var record in array)
        //    {
        //        ManureMaterialType manureMaterialType = new ManureMaterialType
        //        {
        //            Id = Convert.ToInt32(record["id"].ToString()),
        //            Name = record["name"].ToString()
        //        };
        //        manureMaterialTypes.Add(manureMaterialType);
        //    }

        //    return manureMaterialTypes;
        //}

        //public List<SelectListItem> GetManureMaterialTypesDll()
        //{
        //    var manureMaterialTypes = GetManureMaterialTypes();

        //    List<SelectListItem> manureMaterialTypeOptions = new List<SelectListItem>();

        //    foreach (var r in manureMaterialTypes)
        //    {
        //        var li = new SelectListItem()
        //            { Id = r.Id, Value = r.Name };
        //        manureMaterialTypeOptions.Add(li);
        //    }

        //    return manureMaterialTypeOptions;
        //}
    }

}

