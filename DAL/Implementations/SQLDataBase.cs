using BE.Models;
using DAL.DataSources;
using DAL.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Implementations
{
    public class SQLDataBase : Idal
    { 
        /// <summary>
        ///this func add report to db
        /// </summary>
        public bool AddReport(BE.Models.Report report)
        {
            HitsEntities db = new HitsEntities();//create new connection
            try
            {
                db.Reports.Add(new DataSources.Report//add report fields
                {
                    Address = report.Address,
                    AddressCoordinates = report.LatLongLocation,
                    NumberOfBooms = report.NumberOfBooms,
                    ReporterID = report.ReporterID,
                    ReportID = report.ReportID,
                    TimeFalling = report.FallingTime,
                    Updated = report.Updated ? -1 : 0,//to check if its new report or existing
                });
                db.SaveChanges();
                return true;//if success
            }
            catch (DbEntityValidationException)//problem with adding new report
            {
                return false;
            }
        }
        /// <summary>
        ///this func add reporter to db
        /// </summary>
        public bool AddReporter(BE.Models.Reporter reporter)
        {
            HitsEntities db = new HitsEntities();//new connection to db
            try
            {
                db.Reporters.Add(new DataSources.Reporter//add new reporter
                {
                    ReporterID = reporter.ReporterID,
                    ReporterName = reporter.ReporterName,
                    ReporterAddress = reporter.ReporterAddress,
                    LatLongAddress = reporter.LatLongReporterLocation,
                    ReporterProfilePicture = reporter.ReporterProfilePicture
                });
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        ///this func add update report to db
        /// </summary>
        public bool AddUpdatedReport(BE.Models.UpdatedReport updatedReport)
        {
            HitsEntities db = new HitsEntities();//new connection to db
            try
            {
                foreach (var report in db.Reports)//get over all reports in db
                {
                    if (report.ReportID == updatedReport.ReportID)//the report we want upadate
                    {
                        if (report.Updated == 0 || report.Updated == null)//if until now it wasnt updated
                        {
                            db.UpdatedReports.Add(new DataSources.UpdatedReport//add UpdatedReport fields to db
                            {
                                ReportID = updatedReport.ReportID,
                                NewCoordinates = updatedReport.NewCoordinates,
                                NewTime = updatedReport.NewTime,
                                NumberOfHits = updatedReport.NumberOfHits
                            });
                            report.Updated = -1;//to flag the report update
                            break;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                db.SaveChanges();
                return true;
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException)
            {
                return false;
            }
        }
        /// <summary>
        ///this func remove update report to db
        /// </summary>
        public bool RemoveUpdatedReport(int reportID)
        {
            HitsEntities db = new HitsEntities();//create new connection
            DataSources.UpdatedReport toDelete =//we dlete
                (from report in db.UpdatedReports
                 where report.ReportID == reportID//find my updatereport that had the id of the report
                 select report).FirstOrDefault();
            if (toDelete != null)
            {
                db.UpdatedReports.Remove(toDelete);//delete
                db.SaveChanges();
                return true;
            }
            return false;
        }
        /// <summary>
        ///this func return all the reporters
        /// </summary>
        public IEnumerable GetReporters()
        {
            HitsEntities db = new HitsEntities();//create new connection to db
            return
                from reporter in db.Reporters
                select new BE.Models.Reporter//create new entity of reporter
                {
                    //updates the fields
                    LatLongReporterLocation = reporter.LatLongAddress,
                    ReporterAddress = reporter.ReporterAddress,
                    ReporterID = reporter.ReporterID,
                    ReporterName = reporter.ReporterName,
                    ReporterProfilePicture = reporter.ReporterProfilePicture
                };
        }
        /// <summary>
        ///this func return all the reports
        /// </summary>
        public IEnumerable GetReports()
        {
            HitsEntities db = new HitsEntities();//create new connection to db
            return
                from report in db.Reports
                select new BE.Models.Report //create new entity of report
                {   //updates the fields
                    Address = report.Address,
                    FallingTime = report.TimeFalling,
                    LatLongLocation = report.AddressCoordinates,
                    NumberOfBooms = report.NumberOfBooms,
                    ReporterID = report.ReporterID,
                    ReportID = report.ReportID,
                    Updated = report.Updated == null ? false : report.Updated != 0
                };
        }
        /// <summary>
        ///this func return all the update reports
        /// </summary>
        public IEnumerable GetUpdatedReports()
        {
            HitsEntities db = new HitsEntities();//create new connection to db
            return
                from update in db.UpdatedReports
                select new BE.Models.UpdatedReport//create new entity of update report
                {//updates the fields
                    ReportID = update.ReportID,
                    NewCoordinates = update.NewCoordinates
                };
        }
        /// <summary>
        ///this func check if the reporter exists
        /// </summary>
        public bool ReporterIDExists(int reporterID)
        {
            HitsEntities db = new HitsEntities();
            return
                (from reporter in db.Reporters
                 where reporter.ReporterID == reporterID
                 select reporter).FirstOrDefault() != null;
        }
        /// <summary>
        ///this func check if the report exists
        /// </summary>
        public bool ReportIDExists(int reportID)
        {
            HitsEntities db = new HitsEntities();
            return
                (from report in db.Reports
                 where report.ReportID == reportID
                 select report).FirstOrDefault() != null;
        }

        public BE.Models.Report GetReport(int ReportID)
        {
            HitsEntities db = new HitsEntities();
            return
                (from report in db.Reports
                 where report.ReportID == ReportID
                 select new BE.Models.Report
                 {
                     Address = report.Address,
                     FallingTime = report.TimeFalling,
                     LatLongLocation = report.AddressCoordinates,
                     NumberOfBooms = report.NumberOfBooms,
                     ReporterID = report.ReporterID,
                     ReportID = report.ReportID,
                     Updated = report.Updated != 0,
                 }).FirstOrDefault();
        }

        public BE.Models.Reporter GetReporter(int ReporterID)
        {
            HitsEntities db = new HitsEntities();
            return
                (from reporter in db.Reporters
                 where reporter.ReporterID == ReporterID
                 select new BE.Models.Reporter
                 {
                     LatLongReporterLocation = reporter.LatLongAddress,
                     ReporterAddress = reporter.ReporterAddress,
                     ReporterID = reporter.ReporterID,
                     ReporterName = reporter.ReporterName,
                     ReporterProfilePicture = reporter.ReporterProfilePicture
                 }).FirstOrDefault();
        }

        public BE.Models.UpdatedReport GetUpdatedReport(int ReportID)
        {
            HitsEntities db = new HitsEntities();
            return
                (from updatedReport in db.UpdatedReports
                 where updatedReport.ReportID == ReportID
                 select new BE.Models.UpdatedReport
                 {
                     ReportID = updatedReport.ReportID,
                     NewCoordinates = updatedReport.NewCoordinates
                 }).FirstOrDefault();
        }
    }
}
