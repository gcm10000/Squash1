//var nameMusics = [];
//var nameArtist = [];
//var sourceAudio = [];
var array_data = [];

var currentMusic = 0;
var lastMusic = 0;
var music = new Audio();

music.onended = function () {
    //currentMusic
    if (currentMusic != lastMusic)
        NextMusic();

};

function PreviousMusic() {
    StopMusic();
    setTimeout(function () {
        if ((currentMusic < array_data.length)) {
            currentMusic--;
        }
        if (currentMusic < 0)
            currentMusic = array_data.length - 1;

        GetOtherMusic(currentMusic);
        music.src = array_data[currentMusic].source;
        music.play();
    }, 50);
}

function NextMusic() {
    StopMusic();
    setTimeout(function () {
        if ((currentMusic < array_data.length)) {
            currentMusic++;
        }
        if (currentMusic == array_data.length)
            currentMusic = 0;

        GetOtherMusic(currentMusic);
        music.src = array_data[currentMusic].source;
        music.play();
    }, 50);
}

function StopMusic() {
    music.pause();
    music.currentTime = 0;
}

music.onplay = function () {
    //GetOtherMusics(currentMusic);
    $("#play").css("background-image", "url(/svg/media-pause.svg)");
};

music.onpause = function () {
    $("#play").css("background-image", "url(/svg/media-play.svg)");
};

$(document).ready(function () {

    $('.name').each(function (index, obj) {
        //nameMusics.push($(this).text());
        var data_music = new Object();
        data_music.name = $(this).text();
        data_music.artist = '';
        array_data.push(data_music);

    });

    $('.artist').each(function (index, obj) {
        array_data[index].artist = $(this).text();
    });
});

$('#play').click(function () {
    if (music.paused) {
        if (music.src !== '')
            music.play();
    }
    else
        music.pause();
});

$('.backward').click(function () {
    PreviousMusic();
});

$('.forward').click(function () {
    NextMusic();
});

$("button").click(function () {
    var id = $(this).attr('id');
    var duration = $(this).attr('data-duration');
    if ((id !== undefined) && (id.includes("btnPlay-"))) {

        var index = id.replace("btnPlay-", "");
        var btncontent = $(this);

        //stop music
        StopMusic();

        $.ajax({
            url: '/SearchMusic/GetMusic/',
            type: 'get',
            dataType: 'json',
            async: true,
            data: {
                'music': array_data[index].name,
                'artist': array_data[index].artist,
                'duration': duration,
            },
            success: function (data) {

                btncontent.attr("class", "fa fa-pause");

                var source = "http://slider.kz/download/" + data.id + "/" + data.duration + "/" + data.url + "/" + encodeURIComponent(data.tit_art) + ".mp3?extra=" + data.extra;
                music.src = source;
                if (array_data[index].source === undefined) {
                    array_data[index].source = source;
                    currentMusic = index;
                    if (currentMusic === "0")
                        lastMusic = array_data.length - 1;
                    else
                        lastMusic = currentMusic - 1;
                }
                music.play();
            }
        });
    }
});

function GetOtherMusic(index) {
    var dbtn = '#btnPlay-' + index.toString();
    var duration = $(dbtn).attr('data-duration');
    $.ajax({
        url: '/SearchMusic/GetMusic/',
        type: 'get',
        dataType: 'json',
        async: false,
        data: {
            'music': array_data[index].name,
            'artist': array_data[index].artist,
            'duration': duration,
        },
        success: function (data) {
            var source = "http://slider.kz/download/" + data.id + "/" + data.duration + "/" + data.url + "/" + encodeURIComponent(data.tit_art) + ".mp3?extra=" + data.extra;
            if (array_data[index].source === undefined) {
                array_data[index].source = source;
            }
        }
    });
}

