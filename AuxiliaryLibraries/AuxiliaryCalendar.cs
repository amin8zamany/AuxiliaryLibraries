﻿using AuxiliaryLibraries.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryLibraries
{
    /// <summary>
    /// Persian calendar for convertin datetime
    /// </summary>
    public static class AuxiliaryCalendar
    {
        public static long secondTicks = 10000000;
        public static long minuteTicks = 600000000;
        public static long hourTicks = 36000000000;
        public static long dayTicks = 864000000000;
        public static long weekTicks = 6048000000000;
        public static long monthTicks = 25920000000000;
        public static string yearPattern = "yyyy";
        public static string monthPattern = "MM";
        public static string dayPattern = "dd";
        public static string hourPattern = "HH:mm";
        public static string dayOfTheWeekPattern = "EEEE";
        public static string datePattern = "yyyy/MM/dd";
        /// <summary>
        /// Datetime pattern
        /// </summary>
        public static string datetimePattern = "yyyy-MM-dd HH:mm:ss";
        /// <summary>
        /// Utc datetime pattern
        /// </summary>
        public static string utcDatetimePattern = "yyyy-MM-dd'T'hh:mm:ss.SSS";
        public static string IranianTimeZone = "Iran Standard Time";

        /// <summary>
        /// Convert date time to persian (shamsi) date time
        /// </summary>
        /// <param name="dateTime">Miladi date time or Utc date time</param>
        /// <param name="isUtc">If Date is utc send it True, the default valur is False</param>
        /// <param name="delimiter">Separate Year,Month,Day with delimiter, the default valur is '/'</param>
        /// <returns>Persian date as string</returns>
        public static string ToPersianDateTime(this DateTime dateTime, bool isUtc = false, string delimiter = "/")
        {
            System.Globalization.PersianCalendar shamsi = new System.Globalization.PersianCalendar();
            DateTime date = isUtc ? ConvertUTCtoDateTime(dateTime, TimeZoneInfo.FindSystemTimeZoneById(IranianTimeZone)) : dateTime;
            string strdate = null;

            strdate = date.ToString(datePattern);

            var Date = DateTime.Parse(strdate);

            int year = shamsi.GetYear(Date);

            int month = shamsi.GetMonth(Date);

            int day = shamsi.GetDayOfMonth(Date);

            int hour = date.Hour;

            int minute = date.Minute;

            return $"{year}{delimiter}{month}{delimiter}{day} ساعت {hour}:{minute}";
        }

        /// <summary>
        /// Convert date time to persian (shamsi) date without time
        /// </summary>
        /// <param name="dateTime">Miladi date time or Utc date time</param>
        /// <param name="isUtc">If Date is utc send it True, the default valur is False</param>
        /// <param name="delimiter">Separate Year,Month,Day with delimiter, the default valur is '/'</param>
        /// <returns>Persian date as string</returns>
        public static string ToPersianDate(this DateTime dateTime, bool isUtc = false, string delimiter = "/")
        {
            System.Globalization.PersianCalendar shamsi = new System.Globalization.PersianCalendar();
            DateTime date = isUtc ? ConvertUTCtoDateTime(dateTime, TimeZoneInfo.FindSystemTimeZoneById(IranianTimeZone)) : dateTime;
            string strdate = null;

            strdate = date.ToString(datePattern);

            var Date = DateTime.Parse(strdate);

            int year = shamsi.GetYear(Date);

            int month = shamsi.GetMonth(Date);
            string M = month < 10 ? "0" + month : month.ToString();

            int day = shamsi.GetDayOfMonth(Date);
            string D = day < 10 ? "0" + day : day.ToString();

            return $"{year}{delimiter}{M}{delimiter}{D}";
        }

        /// <summary>
        /// Convert date time to persian (shamsi) date time with Second and Millisecond
        /// </summary>
        /// <param name="dateTime">Miladi date time or Utc date time</param>
        /// <param name="isUtc">If Date is utc send it True, the default valur is False</param>
        /// <param name="delimiter">Separate Year,Month,Day with delimiter, the default valur is '/'</param>
        /// <returns>Persian date as string</returns>
        public static string ToPersianFullDateTime(this DateTime dateTime, bool isUtc = false, string delimiter = "/")
        {
            try
            {
                System.Globalization.PersianCalendar shamsi = new System.Globalization.PersianCalendar();
                DateTime date = isUtc ? ConvertUTCtoDateTime(dateTime, TimeZoneInfo.FindSystemTimeZoneById(IranianTimeZone)) : dateTime;

                string strdate = null;

                strdate = date.ToString(datePattern);

                var Date = DateTime.Parse(strdate);

                int year = shamsi.GetYear(Date);

                int month = shamsi.GetMonth(Date);

                int day = shamsi.GetDayOfMonth(Date);

                int hour = date.Hour;

                int minute = date.Minute;

                int second = date.Second;

                int millisecond = date.Millisecond;

                return $"{year}{delimiter}{month}{delimiter}{day} {hour}:{minute}:{second}:{millisecond}";
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Convert Utc date time to Milidai date time
        /// </summary>
        /// <param name="dateTime">Utc date time</param>
        /// <param name="destinationTimeZone">Time zone</param>
        /// <returns>Miladi date time</returns>
        public static DateTime ConvertUTCtoDateTime(DateTime dateTime, TimeZoneInfo destinationTimeZone)
        {
            try
            {
                //TimeZoneInfo destinationTimeZone = TimeZoneInfo.FindSystemTimeZoneById(IranianTimeZone);
                //DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, TimeZoneInfo.Local);
                //Console.WriteLine("The date and time are {0} {1}.",
                //                  cstTime,
                //                  destinationTimeZone.IsDaylightSavingTime(cstTime) ?
                //                          destinationTimeZone.DaylightName : destinationTimeZone.StandardName);
                DateTime date = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
                return TimeZoneInfo.ConvertTimeToUtc(dateTime, destinationTimeZone);
            }
            catch (TimeZoneNotFoundException)
            {
                Console.WriteLine("The registry does not define the Central Standard Time zone.");
            }
            catch (InvalidTimeZoneException)
            {
                Console.WriteLine("Registry data on the Central Standard Time zone has been corrupted.");
            }
            catch (Exception e)
            {
                try
                {
                    DateTime date = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
                    return TimeZoneInfo.ConvertTimeToUtc(date, TimeZoneInfo.Local);
                }
                catch { }
            }
            return DateTime.Now;
        }

        /// <summary>
        /// Return a datetime as midnight time
        /// </summary>
        /// <param name="date">Milidai datetime</param>
        /// <param name="hour">Default value is 0</param>
        /// <param name="min">Default value is 0</param>
        /// <param name="sec">Default value is 0</param>
        /// <param name="mil">Default value is 0</param>
        /// <returns>DateTime as midnight time</returns>
        public static DateTime BeginDate(this DateTime date, int hour = 0, int min = 0, int sec = 0, int mil = 0)
        {
            return new DateTime(date.Year, date.Month, date.Day, hour, min, sec, mil);
        }

        /// <summary>
        /// Return a datetime as the end time of day (Tomorrow midnight, somehow)
        /// </summary>
        /// <param name="date">Milidai datetime</param>
        /// <param name="hour">Default value is 23</param>
        /// <param name="min">Default value is 59</param>
        /// <param name="sec">Default value is 59</param>
        /// <param name="mil">Default value is 999</param>
        /// <returns>DateTime as end time of day</returns>
        public static DateTime EndDate(this DateTime date, int hour = 23, int min = 59, int sec = 59, int mil = 999)
        {
            return new DateTime(date.Year, date.Month, date.Day, hour, min, 59, 999);
        }

        /// <summary>
        /// Convert datetime to date as pretty format.
        /// Samples:
        /// Persian format = هجدهم آذر ماه 1395
        /// Miladi format = Ninth of December 2016
        /// </summary>
        /// <param name="dateTime">Miladi date time or Utc date time</param>
        /// <param name="isUtc">If Date is utc send it True, the default valur is False</param>
        /// <param name="toPersian">If you want to convert to persian date send it true, and for miladi date send false. Default value is true</param>
        /// <returns>string</returns>
        public static string ToPrettyDate(this DateTime dateTime, bool isUtc = false, bool toPersian = true)
        {
            System.Globalization.PersianCalendar shamsi = new System.Globalization.PersianCalendar();
            DateTime date = isUtc ? ConvertUTCtoDateTime(dateTime, TimeZoneInfo.FindSystemTimeZoneById(IranianTimeZone)) : dateTime;

            var result = string.Empty;
            var time = date.Ticks;
            var midNightDate = DateTime.Now.BeginDate();

            //int today = GetDayOfWeek();
            //int passedDayOdWeek = today + 1 <= 7 ? today + 1 : 1;

            long midNightTime = midNightDate.Ticks;

            if (time >= midNightTime)
            {
                result = toPersian ? $"{DisplayNames.TodayPersian} ساعت {date.ToString(hourPattern)}" : $"{DisplayNames.Today} at {date.ToString(hourPattern)}";
            }
            else if (time >= midNightTime - dayTicks)
            {
                result = toPersian ? $"{DisplayNames.YesterdayPersian} ساعت {date.ToString(hourPattern)}" : $"{DisplayNames.Yesterday} at {date.ToString(hourPattern)}";
            }
            //else if (time >= midNightTime - (oneDay * passedDayOdWeek))
            //{
            //    result = GetDayOfWeek(GetDayOfWeek(date.DayOfWeek.ToString()), isPersian);
            //}
            else {
                int year = date.Year, month = date.Month, day = date.Day, hour = date.Hour, minute = date.Minute, second = date.Second, millisecond = date.Millisecond;
                if (toPersian)
                {


                    string strdate = null;

                    strdate = dateTime.ToString(datePattern);

                    var Date = DateTime.Parse(strdate);

                    year = shamsi.GetYear(dateTime);

                    month = shamsi.GetMonth(dateTime);

                    day = shamsi.GetDayOfMonth(dateTime);

                    hour = dateTime.Hour;

                    minute = dateTime.Minute;
                }
                var dayOfWeek = GetDayOfWeek(GetDayOfWeek(date.DayOfWeek.ToString()), toPersian);
                var dayOfMonth = GetDayOfMonth(day, toPersian);
                var monthOfYear = GetMonth(month, toPersian);
                monthOfYear = toPersian ? $"{monthOfYear} ماه" : $"of {monthOfYear}";
                result = $"{dayOfWeek} {dayOfMonth} {monthOfYear} {year}";
            }

            return result;
        }

        /// <summary>
        /// Convert datetime to time as pretty format.
        /// Samples:
        /// Persian format = 18 دقیقه قبل
        /// Miladi format = 18 minutes ago
        /// </summary>
        /// <param name="dateTime">Miladi date time or Utc date time</param>
        /// <param name="isUtc">If Date is utc send it True, the default valur is False</param>
        /// <param name="toPersian">If you want to convert to persian date send it true, and for miladi date send false. Default value is true</param>
        /// <returns>string</returns>
        public static string ToPrettyTime(this DateTime dateTime, bool isUtc = false, bool toPersian = true)
        {
            System.Globalization.PersianCalendar shamsi = new System.Globalization.PersianCalendar();
            DateTime date = isUtc ? ConvertUTCtoDateTime(dateTime, TimeZoneInfo.FindSystemTimeZoneById(IranianTimeZone)) : dateTime;

            var result = string.Empty;
            var diff = DateTime.Now.Ticks - date.Ticks;
            var diffDate = DateTime.Now - date;

            if (diff == 0)
            {
                return toPersian ? DisplayNames.JustRightNowPersian : DisplayNames.JustRightNow;
            }
            else if (diff < minuteTicks)
            {
                int second = Convert.ToInt32(diffDate.TotalSeconds);
                second = second <= 0 ? 1 : second;
                result = toPersian ? string.Format(DisplayNames.SecondsAgoPersian, second) : string.Format(DisplayNames.SecondsAgo, second);
            }
            else if (diff < hourTicks)
            {
                int min = Convert.ToInt32(diffDate.TotalMinutes);
                min = min <= 0 ? 1 : min;
                result = toPersian ? string.Format(DisplayNames.MinutesAgoPersian, min) : string.Format(DisplayNames.MinutesAgo, min);
            }
            else if (diff < dayTicks)
            {
                int hour = Convert.ToInt32(diffDate.TotalHours);
                hour = hour <= 0 ? 1 : hour;
                result = toPersian ? string.Format(DisplayNames.HourAgoPersian, hour) : string.Format(DisplayNames.HourAgo, hour);
            }
            else if (diff < weekTicks)
            {
                int day = Convert.ToInt32(diffDate.TotalDays);
                day = day <= 0 ? 1 : day;
                result = toPersian ? string.Format(DisplayNames.DayAgoPersian, day) : string.Format(DisplayNames.DayAgo, day);
            }
            else if (diff < monthTicks)
            {
                int week = Convert.ToInt32(diffDate.TotalDays / 7);
                week = week <= 0 ? 1 : week;
                result = toPersian ? string.Format(DisplayNames.WeekAgoPersian, week) : string.Format(DisplayNames.WeekAgo, week);
            }
            else {
                int month = Convert.ToInt32(diffDate.TotalDays / 30);
                month = month <= 0 ? 1 : month;
                result = toPersian ? string.Format(DisplayNames.MonthAgoPersian, month) : string.Format(DisplayNames.MonthAgo, month);
            }

            return result;
        }

        public static string GetDayOfWeek(int day, bool toPersian)
        {
            switch (day)
            {
                case 1:
                    {
                        return toPersian ? DisplayNames.SaturdayPersian : DisplayNames.Saturday;
                    }
                case 2:
                    {
                        return toPersian ? DisplayNames.SundayPersian : DisplayNames.Sunday;
                    }
                case 3:
                    {
                        return toPersian ? DisplayNames.MondayPersian : DisplayNames.Monday;
                    }
                case 4:
                    {
                        return toPersian ? DisplayNames.TuesdayPersian : DisplayNames.Tuesday;
                    }
                case 5:
                    {
                        return toPersian ? DisplayNames.WednesdayPersian : DisplayNames.Wednesday;
                    }
                case 6:
                    {
                        return toPersian ? DisplayNames.ThursdayPersian : DisplayNames.Thursday;
                    }
                case 7:
                    {
                        return toPersian ? DisplayNames.FridayPersian : DisplayNames.Friday;
                    }
                default:
                    {
                        return string.Empty;
                    }
            }
        }

        public static string GetDayOfMonth(int day, bool toPersian)
        {
            switch (day)
            {
                case 1:
                    {
                        return toPersian ? DisplayNames.day1Persian : DisplayNames.day1;
                    }
                case 2:
                    {
                        return toPersian ? DisplayNames.day2Persian : DisplayNames.day2;
                    }
                case 3:
                    {
                        return toPersian ? DisplayNames.day3Persian : DisplayNames.day3;
                    }
                case 4:
                    {
                        return toPersian ? DisplayNames.day4Persian : DisplayNames.day4;
                    }
                case 5:
                    {
                        return toPersian ? DisplayNames.day5Persian : DisplayNames.day5;
                    }
                case 6:
                    {
                        return toPersian ? DisplayNames.day6Persian : DisplayNames.day6;
                    }
                case 7:
                    {
                        return toPersian ? DisplayNames.day7Persian : DisplayNames.day7;
                    }
                case 8:
                    {
                        return toPersian ? DisplayNames.day8Persian : DisplayNames.day8;
                    }
                case 9:
                    {
                        return toPersian ? DisplayNames.day9Persian : DisplayNames.day9;
                    }
                case 10:
                    {
                        return toPersian ? DisplayNames.day10Persian : DisplayNames.day10;
                    }
                case 11:
                    {
                        return toPersian ? DisplayNames.day11Persian : DisplayNames.day11;
                    }
                case 12:
                    {
                        return toPersian ? DisplayNames.day12Persian : DisplayNames.day12;
                    }
                case 13:
                    {
                        return toPersian ? DisplayNames.day13Persian : DisplayNames.day13;
                    }
                case 14:
                    {
                        return toPersian ? DisplayNames.day14Persian : DisplayNames.day14;
                    }
                case 15:
                    {
                        return toPersian ? DisplayNames.day15Persian : DisplayNames.day15;
                    }
                case 16:
                    {
                        return toPersian ? DisplayNames.day16Persian : DisplayNames.day16;
                    }
                case 17:
                    {
                        return toPersian ? DisplayNames.day17Persian : DisplayNames.day17;
                    }
                case 18:
                    {
                        return toPersian ? DisplayNames.day18Persian : DisplayNames.day18;
                    }
                case 19:
                    {
                        return toPersian ? DisplayNames.day19Persian : DisplayNames.day19;
                    }
                case 20:
                    {
                        return toPersian ? DisplayNames.day20Persian : DisplayNames.day20;
                    }
                case 21:
                    {
                        return toPersian ? DisplayNames.day21Persian : DisplayNames.day21;
                    }
                case 22:
                    {
                        return toPersian ? DisplayNames.day22Persian : DisplayNames.day22;
                    }
                case 23:
                    {
                        return toPersian ? DisplayNames.day23Persian : DisplayNames.day23;
                    }
                case 24:
                    {
                        return toPersian ? DisplayNames.day24Persian : DisplayNames.day24;
                    }
                case 25:
                    {
                        return toPersian ? DisplayNames.day25Persian : DisplayNames.day25;
                    }
                case 26:
                    {
                        return toPersian ? DisplayNames.day26Persian : DisplayNames.day26;
                    }
                case 27:
                    {
                        return toPersian ? DisplayNames.day27Persian : DisplayNames.day27;
                    }
                case 28:
                    {
                        return toPersian ? DisplayNames.day28Persian : DisplayNames.day28;
                    }
                case 29:
                    {
                        return toPersian ? DisplayNames.day29Persian : DisplayNames.day29;
                    }
                case 30:
                    {
                        return toPersian ? DisplayNames.day30Persian : DisplayNames.day30;
                    }
                case 31:
                    {
                        return toPersian ? DisplayNames.day31Persian : DisplayNames.day31;
                    }
                default:
                    {
                        return string.Empty;
                    }
            }
        }

        public static int GetDayOfWeek(string day = null)
        {
            day = string.IsNullOrEmpty(day) ? DateTime.Now.DayOfWeek.ToString() : day;
            switch (day.ToLower())
            {
                case "saturday":
                    {
                        return 1;
                    }
                case "sunday":
                    {
                        return 2;
                    }
                case "monday":
                    {
                        return 3;
                    }
                case "tuesday":
                    {
                        return 4;
                    }
                case "wednesday":
                    {
                        return 5;
                    }
                case "thursday":
                    {
                        return 6;
                    }
                case "friday":
                    {
                        return 7;
                    }
                default:
                    {
                        return 0;
                    }
            }
        }

        public static string GetMonth(int month, bool toPersian)
        {
            switch (month)
            {
                case 1:
                    return toPersian ? DisplayNames.Month1Persian : DisplayNames.Month1;
                case 2:
                    return toPersian ? DisplayNames.Month2Persian : DisplayNames.Month2;
                case 3:
                    return toPersian ? DisplayNames.Month3Persian : DisplayNames.Month3;
                case 4:
                    return toPersian ? DisplayNames.Month4Persian : DisplayNames.Month4;
                case 5:
                    return toPersian ? DisplayNames.Month5Persian : DisplayNames.Month5;
                case 6:
                    return toPersian ? DisplayNames.Month6Persian : DisplayNames.Month6;
                case 7:
                    return toPersian ? DisplayNames.Month7Persian : DisplayNames.Month7;
                case 8:
                    return toPersian ? DisplayNames.Month8Persian : DisplayNames.Month8;
                case 9:
                    return toPersian ? DisplayNames.Month9Persian : DisplayNames.Month9;
                case 10:
                    return toPersian ? DisplayNames.Month10Persian : DisplayNames.Month10;
                case 11:
                    return toPersian ? DisplayNames.Month11Persian : DisplayNames.Month11;
                case 12:
                    return toPersian ? DisplayNames.Month12Persian : DisplayNames.Month12;
                default:
                    return string.Empty;
            }
        }
    }

    //private class SolarCalendar
    //{

    //    public String strWeekDay = "";
    //    public String strMonth = "";

    //    int date;
    //    int month;
    //    int year;

    //    public SolarCalendar(Context context)
    //    {
    //        Date GregorianDate = new Date();
    //        calculateSolarCalendar(GregorianDate, context);
    //    }

    //    public SolarCalendar(Date GregorianDate, Context context)
    //    {
    //        calculateSolarCalendar(GregorianDate, context);
    //    }

    //    private void calculateSolarCalendar(Date GregorianDate, Context context)
    //    {

    //        int ld;

    //        int gregorianYear = GregorianDate.getYear() + 1900;
    //        int gregorianMonth = GregorianDate.getMonth() + 1;
    //        int gregorianDate = GregorianDate.getDate();
    //        int WeekDay = GregorianDate.getDay();

    //        int[] buf1 = new int[12];
    //        int[] buf2 = new int[12];

    //        buf1[0] = 0;
    //        buf1[1] = 31;
    //        buf1[2] = 59;
    //        buf1[3] = 90;
    //        buf1[4] = 120;
    //        buf1[5] = 151;
    //        buf1[6] = 181;
    //        buf1[7] = 212;
    //        buf1[8] = 243;
    //        buf1[9] = 273;
    //        buf1[10] = 304;
    //        buf1[11] = 334;

    //        buf2[0] = 0;
    //        buf2[1] = 31;
    //        buf2[2] = 60;
    //        buf2[3] = 91;
    //        buf2[4] = 121;
    //        buf2[5] = 152;
    //        buf2[6] = 182;
    //        buf2[7] = 213;
    //        buf2[8] = 244;
    //        buf2[9] = 274;
    //        buf2[10] = 305;
    //        buf2[11] = 335;

    //        if ((gregorianYear % 4) != 0)
    //        {
    //            date = buf1[gregorianMonth - 1] + gregorianDate;

    //            if (date > 79)
    //            {
    //                date = date - 79;
    //                if (date <= 186)
    //                {
    //                    switch (date % 31)
    //                    {
    //                        case 0:
    //                            month = date / 31;
    //                            date = 31;
    //                            break;
    //                        default:
    //                            month = (date / 31) + 1;
    //                            date = (date % 31);
    //                            break;
    //                    }
    //                    year = gregorianYear - 621;
    //                }
    //                else {
    //                    date = date - 186;

    //                    switch (date % 30)
    //                    {
    //                        case 0:
    //                            month = (date / 30) + 6;
    //                            date = 30;
    //                            break;
    //                        default:
    //                            month = (date / 30) + 7;
    //                            date = (date % 30);
    //                            break;
    //                    }
    //                    year = gregorianYear - 621;
    //                }
    //            }
    //            else {
    //                if ((gregorianYear > 1996) && (gregorianYear % 4) == 1)
    //                {
    //                    ld = 11;
    //                }
    //                else {
    //                    ld = 10;
    //                }
    //                date = date + ld;

    //                switch (date % 30)
    //                {
    //                    case 0:
    //                        month = (date / 30) + 9;
    //                        date = 30;
    //                        break;
    //                    default:
    //                        month = (date / 30) + 10;
    //                        date = (date % 30);
    //                        break;
    //                }
    //                year = gregorianYear - 622;
    //            }
    //        }
    //        else {
    //            date = buf2[gregorianMonth - 1] + gregorianDate;

    //            if (gregorianYear >= 1996)
    //            {
    //                ld = 79;
    //            }
    //            else {
    //                ld = 80;
    //            }
    //            if (date > ld)
    //            {
    //                date = date - ld;

    //                if (date <= 186)
    //                {
    //                    switch (date % 31)
    //                    {
    //                        case 0:
    //                            month = (date / 31);
    //                            date = 31;
    //                            break;
    //                        default:
    //                            month = (date / 31) + 1;
    //                            date = (date % 31);
    //                            break;
    //                    }
    //                    year = gregorianYear - 621;
    //                }
    //                else {
    //                    date = date - 186;

    //                    switch (date % 30)
    //                    {
    //                        case 0:
    //                            month = (date / 30) + 6;
    //                            date = 30;
    //                            break;
    //                        default:
    //                            month = (date / 30) + 7;
    //                            date = (date % 30);
    //                            break;
    //                    }
    //                    year = gregorianYear - 621;
    //                }
    //            }

    //            else {
    //                date = date + 10;

    //                switch (date % 30)
    //                {
    //                    case 0:
    //                        month = (date / 30) + 9;
    //                        date = 30;
    //                        break;
    //                    default:
    //                        month = (date / 30) + 10;
    //                        date = (date % 30);
    //                        break;
    //                }
    //                year = gregorianYear - 622;
    //            }

    //        }

    //        strMonth = GetMonth(month, context);
    //        strWeekDay = GetDayOfWeek(WeekDay, context);
    //    }

    //}
}
