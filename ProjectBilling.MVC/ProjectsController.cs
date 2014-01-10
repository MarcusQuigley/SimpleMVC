using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectBilling.DataAccess;
using System.Windows;
namespace ProjectBilling.MVC
{
    public interface IProjectsController
    {
        void ShowProjectsView(Window owner);
        void Update(Project project);
    }

   public class ProjectsController : IProjectsController
    {
       private ProjectsModel _model;

       public ProjectsController(ProjectsModel projectsModel)
       {
           if (projectsModel == null)
               throw new ArgumentNullException("projectsModel");

           _model = projectsModel;
       }

       public void ShowProjectsView(Window owner)
        {
            ProjectsView view = new ProjectsView(this, _model);
            view.Owner = owner;

            view.Show();
        }

        public void Update(Project project)
        {
            _model.UpdateProject(project);
        }
    }
}
