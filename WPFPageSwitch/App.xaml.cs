using MeasurementDomain.Services.DomainLayer;
using SoundDomain.Infrastructure.Repositories;
using SoundDomain.Model.Entities;
using SoundDomain.Model.ValueObjects;
using SoundDomain.Services;
using System.Linq;
using System.Windows;
using UserDomain.Infrastructure.Repositories;
using UserDomain.Model.Entities;

namespace WPFPageSwitch
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            SoundRepositorySingleton.Instance.FindAll().ToList();

            //System.Data.SqlServerCe.SqlCeEngine engine = new System.Data.SqlServerCe.SqlCeEngine("Data Source=|DataDirectory|Sound.sdf");
            //engine.Upgrade();

            //MeasurementService ms = new MeasurementService();
            //ms.GetMeasuremetns();
        }
    }
}
