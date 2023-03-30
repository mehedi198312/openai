using App.Core.OpenAI.Services.Implementations;
using App.Core.OpenAI.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace App.Core.OpenAI.IOC
{

    public static class OpenAIServiceDI
    {
        public static IServiceCollection InjectOpenAIServices(this IServiceCollection services)
        {
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<ICompletionsService, CompletionsService>();
            services.AddScoped<IEditsService, EditsService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IImageVariationService, ImageVariationService>();
            services.AddScoped<IModelsService, ModelsService>();
            services.AddScoped<IEmbeddingsService, EmbeddingsService>();
            services.AddScoped<IAudioService, AudioService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IFineTunesService, FineTunesService>();

            return services;
        }
    }

}
