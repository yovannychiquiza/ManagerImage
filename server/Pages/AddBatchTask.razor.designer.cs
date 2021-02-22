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
    public partial class AddBatchTaskComponent : ComponentBase
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
            batchtask = new ManagerImage.Models.ManagerImage.BatchTask();
        }

        protected async System.Threading.Tasks.Task Form0Submit(ManagerImage.Models.ManagerImage.BatchTask args)
        {
            try
            {
                batchtask.Name = "TaskImageSorting";
                batchtask.LastEjecution = DateTime.Now;
                batchtask.CreationDate = DateTime.Now;
                var managerImageCreateBatchTaskResult = await ManagerImage.CreateBatchTask(batchtask);
                DialogService.Close(batchtask);
            }
            catch (Exception managerImageCreateBatchTaskException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new BatchTask!");
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
