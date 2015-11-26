$(document).ready(function () {
    var source = {
        datatype: "json",
        datafields: [
            { name: 'Name', type: 'string' },
            { name: 'Uploader', type: 'string'},
            { name: 'Wins', type: 'number' },
            { name: 'Mistakes', type: 'number' },
            { name: 'Rating', type: 'number' },
            { name: 'Game', type: 'string' }
        ],
        id: 'Name',
        url: '/Browse/GetGames'
    };
    var dataAdapter = new $.jqx.dataAdapter(source);
    $("#browseList").jqxGrid(
        {
            width: '100%',
            height: '50%',
            source: dataAdapter,
            showfilterrow: true,
            filterable: true,
            theme: 'arctic',
            columns: [
                { text: 'Játék neve', dataField: 'Name', width: '20%' },
                { text: 'Feltöltő', dataField: 'Uploader', width: '20%' },
                { text: 'Jó megoldások', dataField: 'Wins', width: '15%', cellsalign: 'right' },
                { text: 'Rossz megoldások', dataField: 'Mistakes', width: '15%', cellsalign: 'right' },
                { text: 'Átlagos értékelés', dataField: 'Rating', width: '15%', cellsalign: 'right' },
                { text: '', dataField: 'Game', cellsrenderer: renderer, width: '15%' }
            ]
        });
    $("#browseList").on('rowselect', function (event) {
        var rowData = event.args.row;
        $.post("/Game/Game", { name: rowData.Name, uploader: rowData.Uploader, wins: rowData.Wins, mistakes: rowData.Mistakes, rating: rowData.Rating, game: rowData.Game });
    });
});

var renderer = function () {
    return '<input type="submit" value="Játék indítása"/>'
}