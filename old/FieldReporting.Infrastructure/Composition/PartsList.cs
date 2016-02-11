using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace FieldReporting.Infrastructure.Composition
{
    public class PartsList<TTypeToPopulateWith>
    {

#pragma warning disable 649
        [ImportMany]
        public IEnumerable<TTypeToPopulateWith> Items { get; set; }
#pragma warning restore 649

        public PartsList()
        {
            new PartsAssembler().ComposeParts(this);
        }

        public PartsList(Action<TTypeToPopulateWith> initializeAction)
        {
            new PartsAssembler().ComposeParts(this);

            foreach (var item in Items)
            {
                initializeAction(item);
            }
        }
    }
}