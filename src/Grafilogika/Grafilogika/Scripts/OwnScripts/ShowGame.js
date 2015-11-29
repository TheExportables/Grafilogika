var steps = [];

$(document).ready(function () {
    var gameString = $("#gameString").val();
    var gameRows = gameString.split(";");
    var cols = gameRows[0].length;
    var rows = gameRows.length - 1;

    var firstrow = $('<tr class="game" />');
    var colstrings = [];
    var colBlackCtrs = [];
    var firstcols = [];
    var placeholdercol = $('<td class="game" />');
    firstrow.append(placeholdercol);
    for (j = 0; j < cols; ++j) {
        colBlackCtrs[j] = 0;
        colstrings[j] = "";
        var newfirstcol = $('<td class="game" />');
        var pre = $("<pre/>");
        newfirstcol.append(pre);
        firstcols[j] = pre;
        firstrow.append(newfirstcol);
    }
    $("#gametable").append(firstrow);

    for (i = 0; i < rows; ++i) {
        var newrow = $('<tr class="game" />');
        newrow.height(25);
        newrow.width(25);
        var firstcol = $('<td class="game" />');
        var rowstring = "";
        var rowBlackCtr = 0;
        newrow.append(firstcol);
        for (j = 0; j < cols; ++j) {
            var newcol = $('<td class="game" />');
            newcol.height(newrow.height());
            newcol.width(newrow.width());
            if ((i + 1) % 5 == 0 && (j + 1) % 5 == 0)
                var button = $('<input type="button" style="border-bottom: solid #000000; border-right: solid #000000;"/>').addClass('gamecell').click(function () { $(this).toggleClass('black'); steps.push($(this)); document.getElementById("undobtn").disabled = false; });
            else if ((i + 1) % 5 == 0)
                var button = $('<input type="button" style="border-bottom: solid #000000;"/>').addClass('gamecell').click(function () { $(this).toggleClass('black'); steps.push($(this)); document.getElementById("undobtn").disabled = false; });
            else if ((j + 1) % 5 == 0)
                var button = $('<input type="button" style="border-right: solid #000000;"/>').addClass('gamecell').click(function () { $(this).toggleClass('black'); steps.push($(this)); document.getElementById("undobtn").disabled = false; });
            else
                var button = $('<input type="button" />').addClass('gamecell').click(function () { $(this).toggleClass('black'); steps.push($(this)); document.getElementById("undobtn").disabled = false; });
            button.id = "button" + i + j;
            button.width(newcol.width());
            button.height(newcol.height());
            newcol.append(button);
            newrow.append(newcol);
            if (gameRows[i].charAt(j) == "0") {

                if (rowBlackCtr != 0) {
                    rowstring += rowBlackCtr + " ";
                    rowBlackCtr = 0;
                }

                if (colBlackCtrs[j] != 0) {
                    colstrings[j] += colBlackCtrs[j] + " ";
                    colBlackCtrs[j] = 0;
                }

            } else if (gameRows[i].charAt(j) == "1") {
                ++rowBlackCtr;
                colBlackCtrs[j] += 1;
            }
        }
        if (rowBlackCtr != 0)
            rowstring += rowBlackCtr;
        if (rowstring == "")
            rowstring += 0;
        firstcol.text(rowstring);
        $("#gametable").append(newrow);
    }

    for (j = 0; j < cols; ++j) {
        if (colBlackCtrs[j] != 0)
            colstrings[j] += colBlackCtrs[j];
        var newlinestrings = colstrings[j].split(" ");
        var puzzle = "";
        for (x = 0; x < newlinestrings.length; ++x) {
            if (x == newlinestrings.length - 1)
                puzzle += newlinestrings[x];
            else
                puzzle += newlinestrings[x] + "\n";
        }
        if (puzzle == "" || puzzle == "\n")
            puzzle += 0;
        firstcols[j].text(puzzle);
    }
});

function checkSolution() {
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
        var gname = $("#gamename").text();
        var username = $("#currentuser").text();
        $.ajax({
            url: '/Game/CheckGameSolution',
            data: { 'gameName': gname, 'gameSolution': str, 'userName': username },
            type: 'POST',
            success: function (result) {
                if (result.indexOf("Rossz") > -1) {
                    $.fancybox({
                        content: result,
                    });
                }
                else {
                    $.fancybox({
                        content: result,
                        'closeBtn': false,
                        closeClick: false, // prevents closing when clicking INSIDE fancybox
                        helpers: {
                            overlay: { closeClick: false } // prevents closing when clicking OUTSIDE fancybox
                        }
                    });
                }

            }
        });
        return;
    } else
        alert("Üres megoldást nem lehet beküldeni!");
}

function undoLastStep() {
    if (steps.length != 0) {
        var laststep = steps.pop();
        laststep.toggleClass('black');
    }
    if (steps.length == 0) {
        document.getElementById("undobtn").disabled = true;
    }
}
