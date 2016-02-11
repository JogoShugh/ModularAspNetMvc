using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace FieldReporting.Infrastructure.Composition
{
    public class ComposedList<TTypeToPopulateWith>
    {

#pragma warning disable 649
        [ImportMany]
        public IEnumerable<TTypeToPopulateWith> Items { get; set; }
#pragma warning restore 649

        public ComposedList()
        {
            new SimpleComposer().ComposeParts(this);
        }

        public ComposedList(Action<TTypeToPopulateWith> initializeAction)
        {
            new SimpleComposer().ComposeParts(this);

            foreach (var item in Items)
            {
                initializeAction(item);
            }
        }
    }
}