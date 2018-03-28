using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kf.TWBY
{
    /// <summary>
    /// select 'public const int '+ClassName + '=' + convert(varchar(20),EntityID) +';//'+Name from KfMeta_DataEntity where ClassName<>'' AND EntityID in (SELECT ChildID FROM dbo.kfMeta_DataEntity_Tree WHERE ParentID=3000825)  AND EntityID>3000797
    /// </summary>
    public class EntityIDEnum
    {

    }
}
