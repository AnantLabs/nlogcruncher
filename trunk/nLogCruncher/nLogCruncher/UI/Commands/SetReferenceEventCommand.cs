#region Copyright

// The contents of this file are subject to the Mozilla Public License
//  Version 1.1 (the "License"); you may not use this file except in compliance
//  with the License. You may obtain a copy of the License at
//  
//  http://www.mozilla.org/MPL/
//  
//  Software distributed under the License is distributed on an "AS IS"
//  basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//  License for the specific language governing rights and limitations under 
//  the License.
//  
//  The Initial Developer of the Original Code is Robert Smyth.
//  Portions created by Robert Smyth are Copyright (C) 2008.
//  
//  All Rights Reserved.

#endregion

using System;
using System.Windows.Input;
using NoeticTools.nLogCruncher.Domain;


namespace NoeticTools.nLogCruncher.UI.Commands
{
    public class SetReferenceEventCommand : ICommand
    {
        private readonly IEventsFormatterData formatterData;

        public SetReferenceEventCommand(IEventsFormatterData formatterData)
        {
            this.formatterData = formatterData;
        }

        public ICommand Test { get; set; }

        public void Execute(object parameter)
        {
            formatterData.ReferenceLogEvent = (LogEvent) parameter;
            formatterData.TimeFormat = TimeStampFormat.Relative;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}