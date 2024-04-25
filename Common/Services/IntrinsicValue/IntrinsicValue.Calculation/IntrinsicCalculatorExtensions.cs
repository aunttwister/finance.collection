using IntrinsicValue.Calculation.DataSets.Results;
using IntrinsicValue.Calculation.DCFIntrinsicModel;
using IntrinsicValue.Calculation.DCFIntrinsicModel.Commands;
using IntrinsicValue.Calculation.GrahamIntrinsicModel;
using IntrinsicValue.Calculation.GrahamIntrinsicModel.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntrinsicValue.Calculation
{
    public static class IntrinsicCalculatorExtensions
    {
        public static void RegisterIntrinsicCalculatorServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            services.AddTransient<ICalculateIntrinsicServiceStrategy<GrahamIntrinsicModelCommand, GrahamCalculationResult>, GrahamIntrinsicModelService>();
            services.AddTransient<ICalculateIntrinsicServiceStrategy<DCFIntrinsicModelCommand, DCFCalculationResult>, DCFIntrinsicModelService>();
        }
    }
}
