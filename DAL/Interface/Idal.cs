using BE.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface Idal
    {
        bool AddReporter(Reporter reporter);
        bool AddReport(Report report);
        bool AddUpdatedReport(UpdatedReport report);
        bool RemoveUpdatedReport(int reportID);

        bool ReporterIDExists(int reporterID);
        bool ReportIDExists(int reportID);

        Report GetReport(int ReportID);
        Reporter GetReporter(int ReporterID);
        UpdatedReport GetUpdatedReport(int ReportID);

        /**
         * ! ALL THE NEXT GETTERS RETURN COPIES !
         */
        IEnumerable GetReporters();
        IEnumerable GetReports();
        IEnumerable GetUpdatedReports();
    }
}
