using IntrinsicValue.Calculation.DataSets.Results;
using IntrinsicValue.Calculation.DCFIntrinsicModel.Commands;
using IntrinsicValue.Calculation.DCFIntrinsicModel;
using IntrinsicValue.Calculation.GrahamIntrinsicModel.Commands;
using IntrinsicValue.Calculation.GrahamIntrinsicModel;
using IntrinsicValue.Calculation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IntrinsicValue.Calculation.Init.Commands;
using IntrinsicValue.Calculation.Common.Services;

namespace Financial.Collection.Link.IntrinsicValue.Calculation.ServiceRegistar
{
    public static class RegisterValuationService
    {
        public static void AddValuationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(InitCalculationCommand).Assembly));

            services.AddScoped<ICalculateIntrinsicServiceStrategy<GrahamIntrinsicModelCommand, GrahamCalculationResult>, GrahamIntrinsicModelService>();
            services.AddScoped<ICalculateIntrinsicServiceStrategy<DCFIntrinsicModelCommand, DCFCalculationResult>, DCFIntrinsicModelService>();
            services.AddScoped<IPostValuationService, PostValuationService>();
        }
    }
}
