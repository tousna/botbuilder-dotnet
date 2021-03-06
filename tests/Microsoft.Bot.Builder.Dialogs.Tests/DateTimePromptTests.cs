﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Adapters;
using Microsoft.Bot.Schema;
using Microsoft.Recognizers.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Bot.Builder.Dialogs.Tests
{
    [TestClass]
    public class DateTimePromptTests
    {
        [TestMethod]
        public async Task BasicDateTimePrompt()
        {
            var convoState = new ConversationState(new MemoryStorage());
            var dialogState = convoState.CreateProperty<DialogState>("dialogState");

            TestAdapter adapter = new TestAdapter()
                .Use(convoState);

            // Create new DialogSet.
            var dialogs = new DialogSet(dialogState);

            // Create and add number prompt to DialogSet.
            var dateTimePrompt = new DateTimePrompt("DateTimePrompt", defaultLocale: Culture.English);
            dialogs.Add(dateTimePrompt);

            await new TestFlow(adapter, async (turnContext, cancellationToken) =>
            {
                var dc = await dialogs.CreateContextAsync(turnContext);

                var results = await dc.ContinueAsync();
                if (!turnContext.Responded && !results.HasActive && !results.HasResult)
                {
                    var options = new PromptOptions { Prompt = new Activity { Type = ActivityTypes.Message, Text = "What date would you like?" } };
                    await dc.PromptAsync("DateTimePrompt", options);
                }
                else if (!results.HasActive && results.HasResult)
                {
                    var resolution = ((IList<DateTimeResolution>)results.Result).First();
                    var reply = $"Timex:'{resolution.Timex}' Value:'{resolution.Value}'";
                    await turnContext.SendActivityAsync(reply);
                }
            })
            .Send("hello")
            .AssertReply("What date would you like?")
            .Send("5th December 2018 at 9am")
            .AssertReply("Timex:'2018-12-05T09' Value:'2018-12-05 09:00:00'")
            .StartTestAsync();
        }

        [TestMethod]
        public async Task MultipleResolutionsDateTimePrompt()
        {
            var convoState = new ConversationState(new MemoryStorage());
            var dialogState = convoState.CreateProperty<DialogState>("dialogState");

            TestAdapter adapter = new TestAdapter()
                .Use(convoState);

            // Create new DialogSet.
            var dialogs = new DialogSet(dialogState);

            // Create and add number prompt to DialogSet.
            var dateTimePrompt = new DateTimePrompt("DateTimePrompt", defaultLocale: Culture.English);
            dialogs.Add(dateTimePrompt);

            await new TestFlow(adapter, async (turnContext, cancellationToken) =>
            {
                var dc = await dialogs.CreateContextAsync(turnContext);

                var results = await dc.ContinueAsync();
                if (!turnContext.Responded && !results.HasActive && !results.HasResult)
                {
                    var options = new PromptOptions { Prompt = new Activity { Type = ActivityTypes.Message, Text = "What date would you like?" } };
                    await dc.PromptAsync("DateTimePrompt", options);
                }
                else if (!results.HasActive && results.HasResult)
                {
                    var resolutions = (IList<DateTimeResolution>)results.Result;
                    var timexExpressions = resolutions.Select(r => r.Timex).Distinct();
                    var reply = string.Join(" ", timexExpressions);
                    await turnContext.SendActivityAsync(reply);
                }
            })
            .Send("hello")
            .AssertReply("What date would you like?")
            .Send("Wednesday 4 oclock")
            .AssertReply("XXXX-WXX-3T04 XXXX-WXX-3T16")
            .StartTestAsync();
        }

        [TestMethod]
        public async Task DateTimePromptWithValidator()
        {
            var convoState = new ConversationState(new MemoryStorage());
            var dialogState = convoState.CreateProperty<DialogState>("dialogState");

            TestAdapter adapter = new TestAdapter()
                .Use(convoState);

            // Create new DialogSet.
            var dialogs = new DialogSet(dialogState);

            // Create and add number prompt to DialogSet.
            var dateTimePrompt = new DateTimePrompt("DateTimePrompt", CustomValidator, defaultLocale: Culture.English);
            dialogs.Add(dateTimePrompt);

            await new TestFlow(adapter, async (turnContext, cancellationToken) =>
            {
                var dc = await dialogs.CreateContextAsync(turnContext);

                var results = await dc.ContinueAsync();
                if (!turnContext.Responded && !results.HasActive && !results.HasResult)
                {
                    var options = new PromptOptions { Prompt = new Activity { Type = ActivityTypes.Message, Text = "What date would you like?" } };
                    await dc.PromptAsync("DateTimePrompt", options);
                }
                else if (!results.HasActive && results.HasResult)
                {
                    var resolution = ((IList<DateTimeResolution>)results.Result).First();
                    var reply = $"Timex:'{resolution.Timex}' Value:'{resolution.Value}'";
                    await turnContext.SendActivityAsync(reply);
                }
            })
            .Send("hello")
            .AssertReply("What date would you like?")
            .Send("5th December 2018 at 9am")
            .AssertReply("Timex:'2018-12-05' Value:'2018-12-05'")
            .StartTestAsync();
        }

        private Task CustomValidator(ITurnContext turnContext, PromptValidatorContext<IList<DateTimeResolution>> prompt)
        {
            if (prompt.Recognized.Succeeded)
            {
                var resolution = prompt.Recognized.Value.First();
                // re-write the resolution to just include the date part.
                var rewrittenResolution = new DateTimeResolution
                {
                    Timex = resolution.Timex.Split('T')[0],
                    Value = resolution.Value.Split(' ')[0]
                };
                prompt.End(new List<DateTimeResolution> { rewrittenResolution });
            }
            return Task.CompletedTask;
        }
    }
}

