using Hemlock.Models;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Hemlock.Handlers
{
    public class ProjectsViewModelHandler
    {
        public void CalculatePercentageContributionPerCategory(ref ProjectsViewModel projectsViewModel,
                                                               ref Dictionary<SREDCategory, Dictionary<Employee, float>> employeePercentageContributions, 
                                                               DateTime fromDateTime, 
                                                               DateTime toDateTime)
        {
            foreach (SREDCategory c in projectsViewModel.SelectedProject.SREDCategories.OrderBy(cat => cat.CategoryName))
            {
                int categoryHoursTotal = 0;

                projectsViewModel.EmployeeHoursWorkedPerCategory.Add(c, new Dictionary<Employee, int>());
                projectsViewModel.EmployeePercentageContributionPerCategory_String.Add(c, new Dictionary<Employee, string>());
                employeePercentageContributions.Add(c, new Dictionary<Employee, float>());

                foreach (Employee e in c.ProjectEntries.Select(pe => pe.Employee).Distinct().OrderBy(emp => emp.FullName))
                {
                    int employeeTotalHoursForCategory = c.ProjectEntries.Where(pe => pe.Employee == e && pe.DateCreated >= fromDateTime && pe.DateCreated <= toDateTime).Sum(pe => pe.Hours);
                    if (employeeTotalHoursForCategory > 0)
                    {
                        projectsViewModel.EmployeeHoursWorkedPerCategory[c].Add(e, employeeTotalHoursForCategory);
                        employeePercentageContributions[c].Add(e, employeeTotalHoursForCategory);
                        categoryHoursTotal += employeeTotalHoursForCategory;
                    }
                }
                projectsViewModel.TotalHoursPerCategory.Add(c, categoryHoursTotal);
                projectsViewModel.TotalLoggedHours += categoryHoursTotal;
            }
        }

        public void ConvertPercentageContributionsToString(ref ProjectsViewModel projectsViewModel, Dictionary<SREDCategory, Dictionary<Employee, float>> employeeContributionDictionary) {
            List<SREDCategory> listOfCategories = new List<SREDCategory>(employeeContributionDictionary.Keys);
            foreach (var category in listOfCategories)
            {
                float totalCategoryPercentage = 0;
                int nonZeroTotalLoggedHours = projectsViewModel.TotalLoggedHours;
                if (projectsViewModel.TotalLoggedHours == 0)
                {
                    nonZeroTotalLoggedHours = 1;
                }
                List<Employee> listOfEmployees = new List<Employee>(employeeContributionDictionary[category].Keys);
                foreach (var employee in listOfEmployees)
                {
                    var percentageOfHours = employeeContributionDictionary[category][employee] /= nonZeroTotalLoggedHours;
                    projectsViewModel.EmployeePercentageContributionPerCategory_String[category].Add(employee, percentageOfHours.ToString("P1"));
                    totalCategoryPercentage += percentageOfHours;
                }
                projectsViewModel.TotalPercentageOfContributionPerCategory_String.Add(category, totalCategoryPercentage.ToString("P1"));
            }
            
        }

        public void SetViewModelDates(ref ProjectsViewModel projectsViewModel, string fromDate, string toDate)
        {
            DateTime fromDateTime;
            DateTime toDateTime;
            if (String.IsNullOrEmpty(toDate) || String.IsNullOrEmpty(fromDate))
            {
                DateTime firstDayOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                fromDateTime = firstDayOfWeek;
                toDateTime = firstDayOfWeek.AddDays(6);
            }
            else
            {
                fromDateTime = DateTime.Parse(fromDate);
                toDateTime = DateTime.Parse(toDate);
            }

            projectsViewModel.FromDateString = fromDateTime.ToString(@"MM\/dd\/yyyy");
            projectsViewModel.ToDateString = toDateTime.ToString(@"MM\/dd\/yyyy");
        }
    }
}