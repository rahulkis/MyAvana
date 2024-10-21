function onRender(data, type, row, meta) {
    var tr;
    //if (row.IsRevoked == false) {
       // tr = '<a class="btn btn-primary" onclick="openRevokeAccess(\'' + row.Id + '\',\'' + row.SharedWith + '\')" Title="Revoke Access" >View HHCP</a>';
    tr = '<a href="/HairProfile/CustomerHair?HairProfileId=' + row.HairProfileId + '&View=shared" Title="View HHCP" class="edit_customer"><i class="fa fa-eye" aria-hidden="true"></i></a>';

   // }

    return tr;
}