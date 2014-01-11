using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectBilling.DataAccess;

namespace ProjectBilling.MVP
{
    public class ProjectEventArgs : EventArgs
    {
        public Project Project { get; set; }
        public ProjectEventArgs(Project project)
        {
            this.Project = project;
        }
    }

    public interface IModel
    {
        List<Project> GetProjects();
        Project GetProject(int id);
        void UpdateProject(Project project);
        event EventHandler<ProjectEventArgs> ProjectUpdated;
    }

    public class Model : IModel
    {
        private List<Project> _projects;
 
        public Model()
        {
            _projects = new DataServiceStub().GetProjects();
        }

        public List<Project> GetProjects()
        {
            return _projects;
        }

        public Project GetProject(int id)
        {
            return _projects.Where((p)
                => p.ID == id)
                .FirstOrDefault();
        }

        public void UpdateProject(Project project)
        {
            //Project p = GetProject(project.ID);
            //if (p != null)
            //{
            //    p.Actual = project.Actual;
            //    p.Estimate = project.Estimate;
            //    p.Name = project.Name;

                OnProjectUpdated(new ProjectEventArgs(project));
            //}
        }

        public event EventHandler<ProjectEventArgs> ProjectUpdated;

        protected void OnProjectUpdated(ProjectEventArgs args)
        {
            EventHandler<ProjectEventArgs> handler = ProjectUpdated;
            if (handler != null)
                handler(this, args);
        }
    }
}
