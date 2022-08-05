function Delete(Name,url) {
    var flag = confirm("請是否刪除此-< " + Name + " >");
    if (flag) {
        window.location = url;
    }
}
function SearchFunc(model,path) {
    var searchdata = document.getElementById("searchId");
    var searchSelectData = document.getElementById("searchSelectId").value;
    var checkRadioButton1 = document.getElementById("RadioModelId1").checked;
    var checkRadioButton2 = document.getElementById("RadioModelId2").checked;
    if (searchdata.value == "") {
        return;
    }
    if (checkRadioButton1) {
        window.location.replace('/'+path+'/' + model +'/SearchView/?search=' + searchdata.value + '&searchSelect=' + searchSelectData);
        return;
    }
    if (checkRadioButton2) {
        window.location.replace('/' + path +'/' + model +'/FuzzySearchView?search=' + searchdata.value + '&searchSelect=' + searchSelectData);
        return;
    }
}

var options = {
    day: 'numeric',    //(e.g., 1)
    month: 'short',    //(e.g., Oct)
    year: 'numeric',   //(e.g., 2019)
    hour: '2-digit',   //(e.g., 02)
    minute: '2-digit', //(e.g., 02)          
    hour12: false,     // 24 小時制
    timeZone: 'asia/taipei' // 美國/紐約
};

function ShowTime() {
    var NowDate = new Date();
    var d = NowDate.getDay();
 
    //var dayNames = new Array("星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六");
    document.getElementById('showtime').innerHTML = ' &copy;' + NowDate.toLocaleString('en-US',options);
    setTimeout('ShowTime()', 1000);
}

//排序 tableId: 表的id,iCol:第幾列 ；dataType：iCol對應的列顯示數據的數據類型
function sortAble(th, tableId, iCol, dataType) {

    var ascChar = "▲";
    var descChar = "▼";

    var table = document.getElementById(tableId);

    //排序標題加背景色
    for (var t = 0; t < table.tHead.rows[0].cells.length; t++) {
        var th = $(table.tHead.rows[0].cells[t]);
        var thText = th.html().replace(ascChar, "").replace(descChar, "");
        if (t == iCol) {
            th.css("background-color", "#fff");
            th.css("width", "12%");
        }
        else {
            th.css("background-color", "#ccc");
            th.css("width", "-12%");
            th.html(thText);
        }

    }

    var tbody = table.tBodies[0];
    var colRows = tbody.rows;
    var aTrs = new Array;

    //將得到的行放入數組，備用
    for (var i = 0; i < colRows.length; i++) {
        //注：如果要求“分組明細不參與排序”，把下面的注釋去掉即可
        //if ($(colRows[i]).attr("group") != undefined) {
        aTrs.push(colRows[i]);
        //}
    }


    //判斷上一次排列的列和現在需要排列的是否同一個。
    var thCol = $(table.tHead.rows[0].cells[iCol]);
    if (table.sortCol == iCol) {
        aTrs.reverse();
    } else {
        //如果不是同一列，使用數組的sort方法，傳進排序函數
        aTrs.sort(compareEle(iCol, dataType));
    }

    var oFragment = document.createDocumentFragment();
    for (var i = 0; i < aTrs.length; i++) {
        oFragment.appendChild(aTrs[i]);
    }
    tbody.appendChild(oFragment);

    //記錄最后一次排序的列索引
    table.sortCol = iCol;

    //給排序標題加“升序、降序” 小圖標顯示
    var th = $(table.tHead.rows[0].cells[iCol]);
    if (th.html().indexOf(ascChar) == -1 && th.html().indexOf(descChar) == -1) {
        th.html(th.html() + ascChar);
    }
    else if (th.html().indexOf(ascChar) != -1) {
        th.html(th.html().replace(ascChar, descChar));
    }
    else if (th.html().indexOf(descChar) != -1) {
        th.html(th.html().replace(descChar, ascChar));
    }

    //重新整理分組
    var subRows = $("#" + tableId + " tr[parent]");
    for (var t = subRows.length - 1; t >= 0; t--) {
        var parent = $("#" + tableId + " tr[group='" + $(subRows[t]).attr("parent") + "']");
        parent.after($(subRows[t]));
    }
}

//將列的類型轉化成相應的可以排列的數據類型
function convert(sValue, dataType) {
    switch (dataType) {
        case "int":
            return parseInt(sValue, 10);
        case "float":
            return parseFloat(sValue);
        case "date":
            return new Date(Date.parse(sValue));
        case "string":
        default:
            return sValue.toString();
    }
}

//排序函數，iCol表示列索引，dataType表示該列的數據類型
function compareEle(iCol, dataType) {
    return function (oTR1, oTR2) {

        var vValue1 = convert(removeHtmlTag($(oTR1.cells[iCol]).html()), dataType);
        var vValue2 = convert(removeHtmlTag($(oTR2.cells[iCol]).html()), dataType);
        if (vValue1 < vValue2) {
            return -1;
        }
        else {
            return 1;
        }

    };
}

//去掉html標簽
function removeHtmlTag(html) {
    return html.replace(/<[^>]+>/g, "");
}


//jQuery加載完以后，分組行高亮背景，分組明細隱藏
$(document).ready(function () {
    $("tr[group]").css("background-color", "#ff9");
    $("tr[parent]").hide();

});


//點擊分組行時，切換分組明細的顯示/隱藏
function showSub(a) {
    var groupValue = $(a).parent().parent().attr("group");
    var subDetails = $("tr[parent='" + groupValue + "']");
    subDetails.toggle();
}





