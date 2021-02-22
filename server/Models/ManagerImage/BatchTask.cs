using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagerImage.Models.ManagerImage
{
  [Table("BatchTasks", Schema = "dbo")]
  public partial class BatchTask
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id
    {
      get;
      set;
    }
    public string Name
    {
      get;
      set;
    }
    public string Description
    {
      get;
      set;
    }
    public string Parameters
    {
      get;
      set;
    }
    public int IntervalMinutes
    {
      get;
      set;
    }
    public bool IsRunning
    {
      get;
      set;
    }
    public DateTime LastEjecution
    {
      get;
      set;
    }
    public bool IsActive
    {
      get;
      set;
    }
    public DateTime CreationDate
    {
      get;
      set;
    }
  }
}
