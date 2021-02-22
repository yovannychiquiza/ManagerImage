using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using ManagerImage.Models.ManagerImage;
using Microsoft.EntityFrameworkCore;

namespace ManagerImage.Pages
{
    public partial class EditBatchTaskComponent : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager UriHelper { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected ManagerImageService ManagerImage { get; set; }

        [Parameter]
        public dynamic Id { get; set; }

        bool _canEdit;
        protected bool canEdit
        {
            get
            {
                return _canEdit;
            }
            set
            {
                if(!object.Equals(_canEdit, value))
                {
                    _canEdit = value;
                    InvokeAsync(() => { StateHasChanged(); });
                }
            }
        }

        ManagerImage.Models.ManagerImage.BatchTask _batchtask;
        protected ManagerImage.Models.ManagerImage.BatchTask batchtask
        {
            get
            {
                return _batchtask;
            }
            set
            {
                if(!object.Equals(_batchtask, value))
                {
                    _batchtask = value;
                    InvokeAsync(() => { StateHasChanged(); });
                }
            }
        }

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            await Load();
        }
        protected async System.Threading.Tasks.Task Load()
        {
            canEdit = true;

            var managerImageGetBatchTaskByIdResult = await ManagerImage.GetBatchTaskById(int.Parse($"{Id}"));
            batchtask = managerImageGetBatchTaskByIdResult;
        }

        protected async System.Threading.Tasks.Task CloseButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }

        protected async System.Threading.Tasks.Task Form0Submit(ManagerImage.Models.ManagerImage.BatchTask args)
        {
            try
            {
                var managerImageUpdateBatchTaskResult = await ManagerImage.UpdateBatchTask(int.Parse($"{Id}"), batchtask);
                DialogService.Close(batchtask);
            }
            catch (Exception managerImageUpdateBatchTaskException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to update BatchTask");
            }
        }

        protected async System.Threading.Tasks.Task Button3Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
