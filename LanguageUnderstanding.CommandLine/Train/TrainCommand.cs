﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace LanguageUnderstanding.CommandLine.Train
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Json;
    using Models;

    internal class TrainCommand : BaseCommand<TrainOptions>
    {
        public TrainCommand(TrainOptions options)
            : base(options)
        {
        }

        public override int Main()
        {
            this.RunAsync().Wait();
            return 0;
        }

        private async Task RunAsync()
        {
            this.Log("Training NLU service... ", false);
            var trainingUtterances = Serialization.Read<List<LabeledUtterance>>(this.Options.UtterancesPath);
            var entityTypes = Serialization.Read<List<EntityType>>(this.Options.EntityTypesPath);
            await this.LanguageUnderstandingService.TrainAsync(trainingUtterances, entityTypes);
            this.Log("Done.");

            if (this.Options.WriteConfig)
            {
                var serviceConfiguration = LanguageUnderstandingServiceFactory.GetServiceConfiguration(
                    this.Options.Service,
                    this.LanguageUnderstandingService);

                var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"appsettings.{this.Options.Service}.json");
                await File.WriteAllTextAsync(configPath, serviceConfiguration.ToString());
            }
        }
    }
}
