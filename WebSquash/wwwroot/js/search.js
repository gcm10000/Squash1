$(".inputsearch").on('keypress', function (e) {
    if (e.which == 13) {
        //TODO
        GetMusic();
    }
});
$(".btn-search").click(function () {
    if (document.getElementById("inputsearch").style.display != "none") {
        //TODO
        GetMusic();
    }
    $(".btn-menu").hide();
    $(".logo").hide();
    $(".inputsearch").show();
    $(".btn-closesearch").show();
});
$(".btn-closesearch").click(function () {
    $(".btn-menu").show();
    $(".logo").show();
    $(".inputsearch").hide();
    $(".btn-closesearch").hide();
});

function GetMusic() {
    if ($('#inputsearch').val() == "")
        return;
    $.ajax({
        url: '/SearchMusic/MetaData/',
        type: 'get',
        dataType: 'json',
        async: true,
        data: {
            'query': $('#inputsearch').val()
        },
        success: function (data) {

            //alert(JSON.stringify(data[0]));
            //var firstInfo = data[0];
            //var artist = JSON.stringify(firstInfo.Artist).replace(/['"]+/g, '');
            //var track = JSON.stringify(firstInfo.Track).replace(/['"]+/g, '');

            //GET: SearchMusic/GetInfoTrack/?artist=Lighthouse+Famly&track=Loving+Every+Minute

            //var content = '<h2>Principal Resultado</h2><div class="content"><a href="#"><div id="imageMain" class="image">' + artist + '</div></a><p class="titleMain"><a href="#">' + track + '</a></p><p class="subtitleMain"><a href="#">' + artist + '</a> • <a id="albumMain" href="#"></a></p></div>';
            //$("#main").html(content);

            ////var info = GetInfoTrack(artist, track, false);
            //alert("teste2: " + (info));
            //$("#albumMain").html(info.Album);
            //$("#imageMain").css("background-image", "url('" + info.Images[1].text +"')");

            $("#content").html('<section id="main" class="main"></section>');

            var totalLen = data.length;
            for (var i = 0; i < totalLen; i++) {

                
                //var artist = JSON.stringify(data[i].Artist).(/['"]+/g, '');
                //var track = JSON.stringify(data[i].Track).replace(/['"]+/g, '');
                var artist = data[i].Artist;
                var track = data[i].Track;

                if (i == 0) {
                    var content = '<h2>Principal Resultado</h2><div class="content"><a href="/Redirect/' + artist + '/' + track + '"><div id="imageMain" class="image">' + artist + '</div></a><p class="titleMain"><a href="/Redirect/' + artist + '/' + track + '">' + track + '</a></p><p class="subtitleMain"><a href="#">' + artist + '</a> • <a id="albumMain" href="#"></a></p></div>';
                    $("#main").html(content);
                }
                else {
                    //Caso for outro numero é outro content...
                    var content = '<section id="nomain" class="nomain"><div class="content"><a href="/Redirect/' + artist + '/' + track + '"><div  data-idx="' + i + '"  id="imagenoMain" class="image">' + artist + '</div></a><p class="titlenoMain"><a href="/Redirect/' + artist + '/' + track + '">' + track + '</a></p><p class="subtitlenoMain"><a href="#">' + artist + '</a> • <a id="albumnoMain" data-idx="' + i +'" href="#"></a></p></div></section>';
                    $("#content").append(content);
                }
                //!!!!!!!!
                GetInfoTrack(i, artist, track, true);

            }



            /*
            var totalLen = data.length;
            var artistNum = new Object();

            for (var i = 0; i < totalLen; i++) {
                if ((data[i].Artist) in artistNum) {
                    artistNum[(data[i].Artist)] = artistNum[(data[i].Artist)] + 1;
                }
                else {
                    artistNum[(data[i].Artist)] = 1;
                    //alert((data[i].Artist));
                }
            }
            */
            //michael jackson
            /*
             {"Franz Ferdinand":1,"Michael Jackson":17,"MGMT":1,"Casper":1,"The Killers":1,
             "George Michael":1,"Michael Bublé":5,"Gary Jules":1,"Placebo":1,"Julia Michaels":1}
             */
            //se o requsição estiver contida no Name, logo é uma música
            //se o requsição estiver contida no Artista, logo é um artista
            //o que tiver mais repetições no Artista, será o artista principal na pesquisa
            /*
            alert(JSON.stringify(artistNum));
            for (var artist1 in artistNum) {
                var greaterThan = false;
                for (var artist2 in artistNum) {
                    greaterThan = artistNum[artist1] > artistNum[artist2];
                }
                if (greaterThan) {
                    alert(artistNum[artist1] + artist1);
                    break;
                }
            }*/

        }
    });
}

function GetInfoTrack(index, artist, track, async) {
    var result;
    $.ajax({
        url: '/SearchMusic/GetInfoTrack/',
        type: 'get',
        dataType: 'json',
        async: async,
        data: {
            'artist': artist,
            'track': track,
        },
        success: function (data) {

            result = data;
            if (index === 0) {
                $("#albumMain").html(data.Album);
                $("#imageMain").css("background-image", "url('" + data.Images[1].text + "')");
                // data-idx="' + i +'" 
            }
            else {
                //var array = $('a[id^="albumnoMain"]');
                //var total = array.length;
                //array[index].html(data.Album);
                //alert(index);
                $('a[data-idx="' + index + '"]').html(data.Album);
                if ((data.Images != null) && (data.Images[1].text !== "")) {
                    $('div[data-idx="' + index + '"]').css("background-image", "url('" + data.Images[1].text + "')");
                }
            }

            //GET: SearchMusic/GetInfoTrack/?artist=Lighthouse+Famly&track=Loving+Every+Minute
        }
    });
    return result;
}