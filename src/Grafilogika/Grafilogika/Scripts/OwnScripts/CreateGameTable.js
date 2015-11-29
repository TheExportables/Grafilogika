function generateTable() {
    if ($("#gametable").children().length != 0) {
        var c = confirm("Biztosan új táblát szeretne generálni? A mostani beállításai elvesznek!");
        if (c)
            $("#gametable tr").remove();
        else
            return;
    }
    var row = $("#rows").val();
    var col = $("#cols").val();
    for (i = 0; i < row; ++i) {
        var newrow = $('<tr class="game" />');
        newrow.height(25);
        newrow.width(25);
        for (j = 0; j < col; ++j) {
            var newcol = $('<td class="game" />');
            newcol.height(newrow.height());
            newcol.width(newrow.width());
            var button = $('<input type="button"/>').addClass('gamecell').click(function () { $(this).toggleClass('black'); });
            button.id = "button" + i + j;
            button.width(newcol.width());
            button.height(newcol.height());
            newcol.append(button);
            newrow.append(newcol);
        }
        $("#gametable").append(newrow);
    }
}

function sendGame() {
    if ($("#gametable").children().length != 0) {
        var str = "";
        $("#gametable tr").each(function () {
            var columns = $(this).find('td');
            columns.each(function () {
                var button = $(this).find(":button");
                if (button.hasClass("black"))
                    str += 1;
                else if (button.hasClass("gamecell"))
                    str += 0;
            });
            if (str.length > 0) {
                str += ";";
            }
        });
        if (str.indexOf("1") != (-1)) {
            var gname = document.getElementById("gamename").value;
            $.ajax({
                url: '/Upload/CreateOwnGameProcess',
                data: { 'gameName': gname, 'gameSolution': str },
                type: 'POST',
                success: function (result) {
                    if (result.indexOf("Sikeres") > -1) {
                        var url = "/Upload/UploadGameProcess";
                        window.location.href = url;
                    }
                    else {
                        $.fancybox({
                            content: result,
                        });
                    }
                }
            });
            return;
        }
    } else {
        $.fancybox({
            content: "Üres pályát nem lehet a szerverre küldeni!",
        });
    }
}