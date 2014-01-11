using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectBilling.DataAccess;
using System.Windows.Media;

namespace ProjectBilling.MVP
{
    public class Presenter
    {
        private IView view = null;
        private IModel model = null;

        public Presenter(IView projectView, IModel projectModel)
        {
            view = projectView;
            model = projectModel;

            view.DetailsUpdated += view_DetailsUpdated;
            view.ProjectUpdated += view_ProjectUpdated;
            view.SelectionChanged += view_SelectionChanged;

            model.ProjectUpdated += model_ProjectUpdated;

            view.LoadProjects(model.GetProjects());
        }

        private void SetEstimateColor(Project project)
        {
            if (project.ID == view.SelectedProjectID)
            {
                if (project.Actual > project.Estimate)
                    view.SetEstimateColor(Colors.Red);
                else
                    view.SetEstimateColor(Colors.Green);
            }
        }

        void model_ProjectUpdated(object sender, ProjectEventArgs e)
        {
            view.UpdateProject(e.Project);
            SetEstimateColor(e.Project);
        }

        void view_SelectionChanged(object sender, EventArgs e)
        {
            int selectedID = view.SelectedProjectID;
            if (selectedID > view.NONE_SELECTED)
            {
                Project p = model.GetProject(selectedID);
                if (p != null)
                {
                    view.UpdateDetails(p);
                    SetEstimateColor(p);
                    view.EnableControls(true);
                    return;
                }
            }

            view.EnableControls(false);


        }

        void view_ProjectUpdated(object sender, ProjectEventArgs e)
        {
            model.UpdateProject(e.Project);
        }

        void view_DetailsUpdated(object sender, ProjectEventArgs e)
        {
            SetEstimateColor(e.Project);
        }
    }
}
