using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ProjectBilling.DataAccess;

namespace ProjectBilling.MVC
{
    /// <summary>
    /// Interaction logic for ProjectsView.xaml
    /// </summary>
    public partial class ProjectsView : Window
    {
        private IProjectsController _controller;
        private IProjectsModel _model;
        private const int NONE_SELECTED = -1;

        private Project GetSelectedProject()
        {
            return cboProjects.SelectedItem as Project;
        }
        private int GetSelectedProjectId()
        {
            Project project = GetSelectedProject();
            return (project == null)
            ? NONE_SELECTED : project.ID;
        }

        public ProjectsView(IProjectsController controller, IProjectsModel model)
        {
            InitializeComponent();
            this._controller = controller;
            this._model = model;
            this._model.ProjectUpdated += model_ProjectUpdated;

            cboProjects.ItemsSource = _model.Projects;
            cboProjects.DisplayMemberPath = "Name";
            cboProjects.SelectedValuePath = "ID";
        }
        ~ProjectsView()
        {
            MessageBox.Show("ProjectsView collected");
        }

        void model_ProjectUpdated(object sender, ProjectEventArgs e)
        {
            int selectedID = GetSelectedProjectId();
            if (selectedID > NONE_SELECTED)
            {
                if (selectedID == e.Project.ID)
                {
                    UpdateDetails(e.Project);
                }
            }
        }

        private void UpdateDetails(Project project)
        {
            txtActual.Text = project.Actual.ToString();
            txtEstimate.Text = project.Estimate.ToString();

            UpdateEstimateColor(project);
        }

        private void UpdateEstimateColor(Project project)
        {
            Brush brush = project.Actual < project.Estimate
                 ? Brushes.Green : Brushes.Red;
            txtEstimate.Foreground = brush;
        }

        private double GetDouble(string value)
        {
            return double.Parse(value);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            if (_model != null)
                _model.ProjectUpdated -= model_ProjectUpdated;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Project p = new Project() 
            {
                Actual = GetDouble(txtActual.Text),
                Estimate=GetDouble(txtEstimate.Text),
                Name = cboProjects.Text,
                ID = (int) cboProjects.SelectedValue
            };

            _controller.Update(p);
        }

        private void cboProjects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Project p = GetSelectedProject();
            if (p!=null)
            {
                UpdateDetails(p);
            }
        }
    }
}
