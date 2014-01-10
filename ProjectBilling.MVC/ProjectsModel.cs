using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectBilling.DataAccess;

namespace ProjectBilling.MVC
{
    public interface IProjectsModel
    {
        List<Project> Projects { get; set; }
        void UpdateProject(Project project);
        event EventHandler<ProjectEventArgs> ProjectUpdated;
    }

    public class ProjectsModel : IProjectsModel
    {
       // private List<Project> _projects;

        public ProjectsModel()
        {
            Projects = new DataServiceStub().GetProjects();
        }

        public List<Project> Projects {get;set;}

        public void UpdateProject(Project project)
        {
            Project selectedProject = Projects.Where((p)
                => p.ID == project.ID).FirstOrDefault();
         
            if (selectedProject != null)
            {
                selectedProject.Name = project.Name;
                selectedProject.Actual = project.Actual;
                selectedProject.Estimate = project.Estimate;
            }
        }

        public event EventHandler<ProjectEventArgs> ProjectUpdated;

        protected void OnProjectUpdated(Project project)
        {
            EventHandler<ProjectEventArgs> handler = ProjectUpdated;
            if (handler != null)
                handler(this, new ProjectEventArgs(project));
        }
 
    }

    public class ProjectEventArgs : EventArgs
    {
 
        public Project Project { get; set; }

        public ProjectEventArgs(Project project)
        {
            this.Project = project;
        }
    }
}
