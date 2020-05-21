﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WorkOrderEMS.Models
{
    public class FormTypeListModel
    {
        public long FileTypeId { get; set; }
        public string FileType { get; set; }
        public string  FileName { get; set; }
        public string IsActive { get; set; }

    }
    public class UploadedFiles
    {
        public long FileId { get; set; }
        public int FileId1 { get; set; }
        public long? FileTypeId { get; set; }
        public string FileEmployeeId { get; set; }
        public string FileName { get; set; }
        public string FileTypeName { get; set; }
        public string AttachedFileName { get; set; }
        public string EmployeeId { get; set; }
        public string Action { get; set; }
        public long? Id { get; set; }
        public string AttachedFileLink { get; set; }
        public HttpPostedFileBase file { get; set; }
    }
}
