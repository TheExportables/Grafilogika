$(document).ready(function () {
    var gameString = $("#gameString").val();
    //var gameString = "101;010;101;";
    var gameRows = gameString.split(";");
    var cols = gameRows[0].length;
    var rows = gameRows.length-1;
    
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
            var button = $('<input type="button"/>').addClass('gamecell').click(function () { $(this).toggleClass('black'); });
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
        firstcol.text(rowstring);
        $("#gametable").append(newrow);
    }
    
    for (j = 0; j < cols; ++j) {
        if (colBlackCtrs[j] != 0)
            colstrings[j] += colBlackCtrs[j];
        var newlinestrings = colstrings[j].split(" ");
        var puzzle = "";
        for (x = 0; x < newlinestrings.length; ++x) {
            puzzle += newlinestrings[x] + "\n";
        }
        firstcols[j].text(puzzle);
    }
});