﻿using Agri.Data;
using Agri.Models.Configuration;
using Agri.Models.Farm;
using Agri.Models.Settings;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SERVERAPI.Models.Impl;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static SERVERAPI.Pages.Ranch.RanchFeeding.CreateEdit.Command;

namespace SERVERAPI.Pages.Ranch.RanchFeeding
{
    public class CreateEdit : BasePageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public Command Data { get; set; }

        [BindProperty]
        public string FieldName { get; set; }

        public CreateEdit(IMediator mediator) => _mediator = mediator;

        public async Task OnGetCreateAsync(string fieldName)
        {
            FieldName = fieldName;
            Title = fieldName + " Feeding Area - Add Feed/Forage";
            await PopulateData(new Query());
        }

        public async Task OnGetEditAsync(Query query)
        {
            Title = "Edit Field";
            await PopulateData(query);
        }

        private async Task PopulateData(Query query)
        {
            Data = await _mediator.Send(query);
            Data = await _mediator.Send(new LookupDataQuery { PopulatedData = Data });
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Data.PostedElementEvent == "AddFeedForageAnalysis")
            {
                ModelState.Clear();
                Data.PostedElementEvent = "None";
                Data.StateChanged = true;

                var newId = Data.FeedForageAnalyses.Count + 1;
                Data.FeedForageAnalyses.Add(new Command.FeedForageAnalysis { Id = newId });
            }
            else
            {
                if (ModelState.IsValid)
                {
                    await _mediator.Send(Data);
                    return RedirectToPage(nameof(Index));
                }
            }
            Data = await _mediator.Send(new LookupDataQuery { PopulatedData = Data });
            return Page();
        }

        public class Query : IRequest<Command>
        {
            public int Id { get; set; }
        }

        public class LookupDataQuery : IRequest<Command>
        {
            public Command PopulatedData { get; set; }
        }

        [BindProperties]
        public class Command : IRequest<MediatR.Unit>
        {
            public string FieldName { get; set; }

            public List<FeedingArea> feedingAreas { get; set; }
            public List<FeedForageAnalysis> FeedForageAnalyses { get; set; } = new List<FeedForageAnalysis>();
            public string PostedElementEvent { get; set; }
            public bool StateChanged { get; set; }

            public class FeedingArea
            {
                public string DailyFeedWarning { get; set; }
                public bool isAvailable { get; set; }
                public string FeedName { get; set; }
                public string FeedType { get; set; }
                public string ForageName { get; set; }
                public bool IsBookAnalysis { get; set; }
                public bool IsCustomValues { get; set; }
            }

            public class FeedForageAnalysis
            {
                public int Id { get; set; }
                public int FeedForageTypeId { get; set; }
                public int FeedForageId { get; set; }
                public bool UseBookValues { get; set; }
                public decimal DryMatterPercent { get; set; }
                public decimal CrudeProteinPercent { get; set; }
                public decimal Phosphorus { get; set; }
                public decimal Potassium { get; set; }
                public decimal PercentOfTotalFeedForageToAnimals { get; set; }
                public decimal PercentOfFeedForageWastage { get; set; }
            }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            private readonly IOptions<AppSettings> _appSettings;

            public CommandValidator(IOptions<AppSettings> appSettings)
            {
                _appSettings = appSettings;
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Field, Command>();
            }
        }

        public class Handler : IRequestHandler<Query, Command>
        {
            private readonly UserData _ud;
            private readonly IMapper _mapper;

            public Handler(UserData ud, IMapper mapper)
            {
                _ud = ud;
                _mapper = mapper;
            }

            public async Task<Command> Handle(Query request, CancellationToken cancellationToken)
            {
                var command = new Command();
                if (request.Id != 0)
                {
                    var field = _ud.GetFieldDetailById(request.Id);
                    command = _mapper.Map<Command>(field);
                    //command.FieldName = field.fieldName;
                    //command.Id = field.Id;
                    //command.FieldArea = field.area.ToString("G29");
                    //command.IsSeasonalFeedingArea = field.IsSeasonalFeedingArea;
                    //command.SeasonalFeedingArea = field.SeasonalFeedingArea;
                    //command.FieldComment = field.comment;
                    //command.SelectPrevYrManureOption = field.prevYearManureApplicationFrequency;
                    //command.FeedingValueDays = field.FeedingValueDays;
                    //command.FeedingPercentage = field.FeedingPercentage;
                    //command.MatureAnimalAverage = field.MatureAnimalAverage;
                    //command.MatureAnimalCount = field.MatureAnimalCount;
                    //command.GrowingAnimalAverage = field.GrowingAnimalAverage;
                    //command.GrowingAnimalCount = field.GrowingAnimalCount;

                    //command.FeedingValueDays = field.feedingValueDays;
                }
                else
                {
                    command.FeedForageAnalyses.Add(new Command.FeedForageAnalysis
                    {
                        Id = 1
                    });
                }

                return await Task.FromResult(command);
            }
        }

        public class LookupDataHandler : IRequestHandler<LookupDataQuery, Command>
        {
            private readonly IAgriConfigurationRepository _sd;

            public LookupDataHandler(IAgriConfigurationRepository sd)
            {
                _sd = sd;
            }

            public async Task<Command> Handle(LookupDataQuery request, CancellationToken cancellationToken)
            {
                var command = request.PopulatedData;
                //command.feedingAreas = new List<FeedingArea>();
                //command.feedingAreas.Add(new FeedingArea()
                //{
                //    FeedName = "Feed 1",
                //    isAvailable = true
                //});

                //command.feedingArea.Add(new FeedingArea()
                //{
                //    FeedName = "Feed 2",
                //    isAvailable = false
                //});
                //command.feedingArea.Add(new FeedingArea()
                //{
                //    FeedName = "Feed 3",
                //    isAvailable = false
                //});
                //command.SelectPrevYrManureOptions = _sd.GetPrevManureApplicationInPrevYears();
                //command.SelectDailyFeedOptions = _sd.GetDailyFeedRequirement();
                //if (command.SelectMatureAnimalDailyFeed == null)
                //{
                //    command.SelectMatureAnimalDailyFeed = command.SelectDailyFeedOptions[0].Name;
                //}
                //if (command.SelectGrowingAnimalDailyFeed == null)
                //{
                //    command.SelectGrowingAnimalDailyFeed = command.SelectDailyFeedOptions[0].Name;
                //}
                //command.Placehldr = _sd.GetUserPrompt("fieldcommentplaceholder");
                //command.DailyFeedWarning = _sd.GetUserPrompt("DailyFeedWarning");
                return await Task.FromResult(command);
            }
        }

        public class CommandHandler : IRequestHandler<Command, MediatR.Unit>
        {
            private readonly UserData _ud;
            private readonly IMapper _mapper;
            private readonly IOptions<AppSettings> _appSettings;

            public CommandHandler(UserData ud, IMapper mapper, IOptions<AppSettings> appSettings)
            {
                _ud = ud;
                _mapper = mapper;
                _appSettings = appSettings;
            }

            public async Task<MediatR.Unit> Handle(Command message, CancellationToken cancellationToken)
            {
                var field = new Field();
                field = _mapper.Map<Field>(message);

                if (field.Id == 0)//Need to check here
                {
                    _ud.AddField(field);
                }
                else
                {
                    _ud.UpdateField(field);
                }

                return await Task.FromResult(new MediatR.Unit());
            }
        }
    }
}