using Calculation.Intrinsic.DataSets.GrahamIntrinsicModel;
using Calculation.Intrinsic.GrahamIntrinsicModel;
using Calculation.Intrinsic.GrahamIntrinsicModel.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Calculation.Intrinsic
{
    public static class IntrinsicCalculatorExtensions
    {
        public static void RegisterIntrinsicCalculatorServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            services.AddTransient<ICalculateIntrinsicServiceStrategy<GrahamIntrinsicModelCommand, GrahamIntrinsicModelDataSet>, GrahamIntrinsicModelService>();
        }
    }
}
