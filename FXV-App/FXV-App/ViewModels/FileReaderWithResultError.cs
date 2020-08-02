using System;
using System.Collections.Generic;

namespace FXV.ViewModels
{
    public class FileReaderWithResultError
    {
        public List<ErrorDetail> StoreUsersFaild { get; set; } = new List<ErrorDetail>();
        public List<ErrorDetail> AssignUsersToEventFaild { get; set; } = new List<ErrorDetail>();
        public List<ErrorDetail> AssignUsersToTeamFaild { get; set; } = new List<ErrorDetail>();
        public List<ErrorDetail> AssignUsersToOrgFaild { get; set; } = new List<ErrorDetail>();
        public List<ErrorDetail> StoreTestResultsFaild { get; set; } = new List<ErrorDetail>();
        public List<ErrorDetail> StoreCombineResultsFaild { get; set; } = new List<ErrorDetail>();
        public List<ErrorDetail> StoreEventResultsFaild { get; set; } = new List<ErrorDetail>();

    }

    public class ErrorDetail
    {
        public string UserAccount { get; set; }
        public string Column { get; set; }
        public string Exception { get; set; }
    }
}
