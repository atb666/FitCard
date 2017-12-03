using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BL;
using PersonDemo;
using BL.BusinessLayer;
using System.Configuration;

namespace WebApplication
{
    public partial class _Default : System.Web.UI.Page
    {
        private EstablishmentManager establishmentManager = new EstablishmentManager();
        private StateManager stateManager = new StateManager();
        private CityManager cityManager = new CityManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.RefreshGrid();
                this.RefreshComboState();
                this.RefreshComboCategory();
                this.RefreshComboStatus();
            }
        }

        private void RefreshGrid()
        {
            List<Establishment> data = establishmentManager.ReadAll();

            this.grvEstablishment.DataSource = data;
            this.grvEstablishment.DataBind();

            this.updtGrid.Update();
        }
        private void RefreshComboState()
        {
            List<State> data = stateManager.ReadAll();

            this.ddlState.DataTextField = Utils.GetPropertyName<State>(x => x.Name);
            this.ddlState.DataValueField = Utils.GetPropertyName<State>(x => x.Id);
            this.ddlState.DataSource = data.OrderBy(x => x.Name);
            this.ddlState.DataBind();

            this.ddlState.Items.Insert(0, new ListItem("Selecione", "0"));

            this.RefreshComboCity();
        }
        private void RefreshComboCity()
        {
            int stateId = Convert.ToInt32(this.ddlState.SelectedValue);
            if (stateId == 0)
            {
                this.ddlCity.Items.Clear();
                this.ddlCity.Items.Insert(0, new ListItem("Selecione", "0"));
                this.ddlCity.Enabled = false;
            }
            else
            {
                List<City> data = cityManager.ReadByStateId(stateId);

                this.ddlCity.DataTextField = Utils.GetPropertyName<City>(x => x.Name);
                this.ddlCity.DataValueField = Utils.GetPropertyName<City>(x => x.Id);
                this.ddlCity.DataSource = data.OrderBy(x => x.Name);
                this.ddlCity.DataBind();

                this.ddlCity.Items.Insert(0, new ListItem("Selecione", "0"));
                this.ddlCity.Enabled = true;
            }
        }
        private void RefreshComboCategory()
        {
            this.ddlCategory.Items.Add(new ListItem("Supermercado", ((int)EstablishmentCategory.Supermarket).ToString()));
            this.ddlCategory.Items.Add(new ListItem("Restaurante", ((int)EstablishmentCategory.Restaurant).ToString()));
            this.ddlCategory.Items.Add(new ListItem("Borracharia", ((int)EstablishmentCategory.TireRepairShop).ToString()));
            this.ddlCategory.Items.Add(new ListItem("Posto", ((int)EstablishmentCategory.FuelStation).ToString()));
            this.ddlCategory.Items.Add(new ListItem("Oficina", ((int)EstablishmentCategory.CarRepairShop).ToString()));
        }
        private void RefreshComboStatus()
        {
            this.ddlStatus.Items.Add(new ListItem("Ativo", ((int)EstablishmentStatus.Actived).ToString()));
            this.ddlStatus.Items.Add(new ListItem("Inativo", ((int)EstablishmentStatus.Deactivated).ToString()));
        }

        private void Save()
        {
            Establishment establishment = new Establishment();

            if (!string.IsNullOrEmpty(this.hdfHidden.Value))
            {
                establishment = this.establishmentManager.ReadById(Convert.ToInt64(this.hdfHidden.Value));
            }

            establishment.CompanyName = this.tbxCompanyName.Text.Trim();
            establishment.TradingName = this.tbxTradingName.Text.Trim();
            establishment.Cnpj = this.tbxCnpj.Text.Trim();
            establishment.Mail = this.tbxMail.Text.Trim();
            establishment.Address = this.tbxAddress.Text.Trim();
            establishment.CityId = this.ddlCity.SelectedValue == "0" ? (int?)null : Convert.ToInt32(this.ddlCity.SelectedValue);
            establishment.Phone = this.tbxPhone.Text.Trim();
            establishment.Category = (EstablishmentCategory)Convert.ToInt32(this.ddlCategory.SelectedValue);
            establishment.Status = (EstablishmentStatus)Convert.ToInt32(this.ddlStatus.SelectedValue);

            DateTime registrationDate;
            if (DateTime.TryParse(this.tbxRegistrationDate.Text.Trim(), out registrationDate))
            {
                establishment.RegistrationDate = registrationDate;
            }
            else
            {
                establishment.RegistrationDate = null;
            }

            try
            {
                if (establishment.Id.HasValue)
                {
                    establishmentManager.Update(establishment);
                }
                else
                {
                    establishmentManager.Create(establishment);
                }
            }
            catch (InvalidOperationException ex)
            {
                string script = string.Format("updateTips('{0}');", ex.Message);
                ScriptManager.RegisterClientScriptBlock(this.updtDialog, this.updtDialog.GetType(), "tips", script, true);
                return;
            }

            this.RefreshGrid();
            ScriptManager.RegisterClientScriptBlock(this.updtDialog, this.updtDialog.GetType(), "close", "closeEstablishment();", true);
        }

        protected void ddlState_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this.RefreshComboCity();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            this.Save();
        }
        protected void grvEstablishment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Establishment establishment = (Establishment)e.Row.DataItem;

                if (establishment.CityId.HasValue)
                {
                    City city = cityManager.ReadById(establishment.CityId.Value);
                    ((Label)e.Row.FindControl("lblCity")).Text = city.Name;

                    State state = stateManager.ReadById(city.StateId);
                    ((Label)e.Row.FindControl("lblState")).Text = state.Name;
                }
                if (establishment.RegistrationDate.HasValue)
                {
                    ((Label)e.Row.FindControl("lblRegistrationDate")).Text = establishment.RegistrationDate.Value.ToLocalTime().ToShortDateString();
                }

                Label lblCategory = ((Label)e.Row.FindControl("lblCategory"));
                switch (establishment.Category)
                {
                    case EstablishmentCategory.Supermarket:
                        lblCategory.Text = "Supermercado";
                        break;
                    case EstablishmentCategory.Restaurant:
                        lblCategory.Text = "Restaurante";
                        break;
                    case EstablishmentCategory.TireRepairShop:
                        lblCategory.Text = "Borracharia";
                        break;
                    case EstablishmentCategory.FuelStation:
                        lblCategory.Text = "Posto";
                        break;
                    case EstablishmentCategory.CarRepairShop:
                        lblCategory.Text = "Oficina";
                        break;
                }
                Label lblStatus = ((Label)e.Row.FindControl("lblStatus"));
                switch (establishment.Status)
                {
                    case EstablishmentStatus.Actived:
                        lblStatus.Text = "Ativo";
                        break;
                    case EstablishmentStatus.Deactivated:
                        lblStatus.Text = "Inativo";
                        break;
                }
            }
        }
        protected void lbtnEdit_Click(object sender, EventArgs e)
        {
            LinkButton linkButton = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkButton.NamingContainer;

            long id = Convert.ToInt32(this.grvEstablishment.DataKeys[row.RowIndex]["Id"]);
            Establishment establishment = establishmentManager.ReadById(id);

            this.tbxCompanyName.Text = establishment.CompanyName;
            this.tbxTradingName.Text = establishment.TradingName;
            this.tbxCnpj.Text = establishment.Cnpj;
            this.tbxMail.Text = establishment.Mail;
            this.tbxPhone.Text = establishment.Phone;
            this.tbxRegistrationDate.Text = establishment.RegistrationDate.HasValue ? establishment.RegistrationDate.Value.ToShortDateString() : null;
            this.ddlCategory.SelectedValue = ((int)establishment.Category).ToString();
            this.ddlStatus.SelectedValue = ((int)establishment.Status).ToString();

            if (establishment.CityId.HasValue)
            {
                City city = cityManager.ReadById(establishment.CityId.Value);

                this.ddlState.SelectedValue = city.StateId.ToString();
                this.RefreshComboCity();
                this.ddlCity.SelectedValue = city.Id.ToString();
            }
            else
            {
                this.ddlState.SelectedValue = "0";
                this.RefreshComboCity();
            }

            this.hdfHidden.Value = id.ToString();
            this.updtHiddenFields.Update();
            this.updtDialog.Update();
            ScriptManager.RegisterClientScriptBlock(this.updtHiddenFields, this.updtHiddenFields.GetType(), "close", "editEstablishment();", true);
        }
        protected void lbtnDelete_Click(object sender, EventArgs e)
        {
            LinkButton linkButton = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkButton.NamingContainer;

            long id = Convert.ToInt32(this.grvEstablishment.DataKeys[row.RowIndex]["Id"]);
            establishmentManager.Delete(id);

            this.RefreshGrid();
        }
    }
}
