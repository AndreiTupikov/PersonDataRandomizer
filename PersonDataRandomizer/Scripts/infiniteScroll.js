var loading = false;
var country = 1;
var seed = 1;
var page = 1;
var errors = 0;
const symbols = [" АаБбВвГгДдЕеЁёЖжЗзИиЙйКкЛлМмНнОоПпРрСсТтУуФфХхЦцЧчШшЩщЪъЫыЬьЭэЮюЯя1234567890", " AaáBbCcDdEeéFfGgHhIiíJjKkLlMmNnÑñOoóPpQqRrSsTtUuúüVvWwXxYyZz1234567890", " AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz1234567890"];

(function () {
    setTimeout(() => { refresh() }, 100);
    $(window).scroll(function () {
        if ($(window).scrollTop() + 1 >= $(document).height() - $(window).height() & !loading) {
            loading = true;
            loadItems();
        }
    });
})()

async function loadItems() {
    page++;
    await $.ajax({
        type: 'GET',
        url: '/Home/NextPage?country=' + country + '&page=' + page,
        success: function (data, textstatus) {
            $("#scrolList").append(data);
            $("#originalList").append(data);
        }
    });
    if (errors > 0) await errorsAdd();
    loading = false;
}

function seedChange(x) {
    if (x == 2) seed = Math.floor(Math.random() * 10000000);
    else {
        var s = document.getElementById('seed').value;
        if (s < 0) seed = 0;
        else if (s < 100000000) seed = Math.floor(s);
        else seed = 100000000;
    }
    document.getElementById('seed').value = seed;
    refresh();
}

async function refresh() {
    page = 1;
    var countries = document.getElementsByName('country');
    for (i = 0; i < countries.length; i++) {
        if (countries[i].checked) {
            country = countries[i].value;
            break;
        }
    }
    seed = document.getElementById('seed').value;
    await $.ajax({
        type: 'GET',
        url: '/Home/NextPage?country=' + country + '&seed=' + seed,
        success: function (data, textstatus) {
            $("#scrolList").html(data);
            $("#originalList").html(data);
        }
    });
    await loadItems();
}

function makeErrors(x) {
    if (x == 1) {
        var e = document.getElementById('errorsN').value;
        if (e < 0) errors = 0;
        else if (e > 1000) errors = 1000;
        else errors = Math.floor(e) + (Math.round(Math.round(e % 1 * 100) / 25) * 25) / 100;
    } else errors = document.getElementById('errorsR').value;
    document.getElementById('errorsN').value = errors;
    if (errors > 10) document.getElementById('errorsR').value = 10;
    else document.getElementById('errorsR').value = errors;
    errorsAdd();
}

async function errorsAdd() {
    const originals = await document.querySelector('.originalList').querySelectorAll('.table-row');
    document.getElementById('scrolList').innerHTML = "";
    const errorsCountRandom = await new Math.seedrandom(seed + country);
    console.log(originals)
    for (let original of originals) {
        var elements = await original.cloneNode(true);
        let number = elements.children[0].innerHTML;
        var errorsCount = await Math.floor(errors);
        if (errorsCountRandom() < errors % 1) await errorsCount++;
        var errorsByFields = await [0, 0, 0];
        const errorsByFieldRandom = await new Math.seedrandom(seed + country + number);
        for (let i = 0; i < errorsCount; i++) {
            var r = await Math.floor(errorsByFieldRandom() * 6)
            if (r < 2) await errorsByFields[0]++;
            else if (r < 5) await errorsByFields[1]++;
            else await errorsByFields[2]++;
        }
        for (let j = 0; j < errorsByFields.length; j++) {
            var element = await elements.children[1];
            for (let k = 0; k < errorsByFields[j]; k++) {
                element.innerHTML = await newError(element.innerHTML, number, j, k);
            }
            await elements.append(element);
        }
        await document.getElementById('scrolList').insertAdjacentElement("beforeend", elements);
    }
}

async function newError(str, rowNumber, fieldNumber, errorNumber) {
    const nextRandom = await new Math.seedrandom(seed + country + rowNumber + fieldNumber + errorNumber);
    var index = Math.floor(nextRandom() * str.length);
    var symbol = symbols[country - 1].charAt(Math.floor(nextRandom() * symbols[country - 1].length));
    switch (Math.floor(nextRandom() * 3)) {
        case 0:
            return str.substring(0, index) + symbol + str.substring(index);
            break;
        case 1:
            return str.substring(0, index) + str.substring(index+1)
            break;
        case 2:
            return str.substring(0, index) + symbol + str.substring(index + 1)
            break;
    }
}