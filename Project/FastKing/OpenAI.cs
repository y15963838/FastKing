using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using OpenAI.GPT3;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;

namespace FastKing
{
    public class OpenAI
    {
        const string OPENAPI_TOKEN = "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";
        OpenAIService service;

        public OpenAI()
        {
            service = new OpenAIService(new OpenAiOptions() { ApiKey = OPENAPI_TOKEN });
        }

        public async Task<string> ChatSimple(string q)
        {
            CompletionCreateRequest createRequest = new CompletionCreateRequest()
            {
                Prompt = q,
                Temperature = 0.3f,
                MaxTokens = 1000,
                TopP = 1,
                FrequencyPenalty = 0.0f,
                PresencePenalty = 0.6f,
            };

            var res = await service.Completions.CreateCompletion(createRequest, Models.TextDavinciV3);

            if (res.Successful)
            {
                var t = res.Choices.FirstOrDefault();
                return t == null ? "返回null" : t.Text;
            }
            else
                return "返回错误";
        }

        ChatCompletionCreateRequest ccr;
        public async Task<string> ChatAdvance(string str, bool isNewChat = true)
        {
            if (isNewChat)
            {
                ccr = new ChatCompletionCreateRequest
                {
                    Messages = new List<ChatMessage>{
                    ChatMessage.FromUser(str)
                },

                    Model = Models.Gpt_4,
                    MaxTokens = 4096
                };
            }
            else
            {
                ccr.Messages.Add(ChatMessage.FromUser(str));
            }

            var completionResult = await service.ChatCompletion.CreateCompletion(ccr);

            if (completionResult.Successful)
            {
                return completionResult.Choices.First().Message.Content;
            }
            else
                return "返回错误";
        }

        /// <summary>
        /// 返回图片url数组
        /// </summary>
        public async Task<List<string>> DALLE(string str)
        {
            var imageResult = await service.Image.CreateImage(new ImageCreateRequest
            {
                Prompt = str,
                N = 4,
                Size = StaticValues.ImageStatics.Size.Size1024,
                ResponseFormat = StaticValues.ImageStatics.ResponseFormat.Url,
                User = "TestUser"
            });

            if (imageResult.Successful)
            {
                var f = imageResult.Results.Select(r => r.Url);
                return f.ToList();
            }
            else
                return null;
        }
    }
}