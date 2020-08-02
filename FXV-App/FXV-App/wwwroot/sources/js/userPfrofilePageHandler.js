var profileMenu = new Vue({
        el: '#profile-menu',
        data: {
            user_id: '',
            url_edit:'/UserProfile/Edit'
        },
        mounted() {
            this.user_id = $('#profile-menu').attr('data-uid');
        },
        methods: {
            Edit: function () {
                axios.get(profileMenu.url_edit + '?user_id=' + profileMenu.user_id)
                    .then(response => {
                    var str = response.data.toString();
                    if (str.indexOf("<!--This is the login layout-->") == 0) {
                        alert("The system detects that you have not operated for a long time, please login again");
                        document.clear();
                        location.reload();
                    }
                    else {
                        try {
                            var json = JSON.parse(response.data);
                            if (!json.Result) {
                                SuccessTurnToParentPage(json.Result, false);
                            }
                        }
                        catch (e) {
                            $("#body-content").html(response.data);
                        }
                    }
                }).catch(error => {
                    $("#body-content").html(error.response);
                });
            },
        },
    });

    Vue.component('athlete_biology', {
        template: '\
                <div class="row text-center text-muted">\
                    <div class="col-6">\
                        <h3 class="text-FXV medium-bold-400 mt-1">{{height}}</h3>\
                        <p>Height</p>\
                    </div><div class="col-6">\
                        <h3 class="text-FXV medium-bold-400 mt-1">{{weight}}</h3>\
                        <p>Weight</p>\
                    </div><div class="col-6">\
                        <h3 class="text-FXV medium-bold-400 mt-1">{{wingspan}}</h3>\
                        <p>Wingspan</p>\
                    </div><div class="col-6">\
                        <h3 class="text-FXV medium-bold-400 mt-1">{{handspan}}</h3>\
                        <p>Handspan</p>\
                    </div><div class="col-12">\
                        <h3 class="text-FXV medium-bold-400 mt-1">{{standingreach}}</h3>\
                        <p>Standing Reach</p>\
                    </div><div class="col-12">\
                        <h3 class="text-FXV medium-bold-400 mt-1">{{dominanthand}}</h3>\
                        <p>Dominant Hand</p>\
                    </div>\
                </div>\
      ',
        props: ['height',
            'weight',
            'wingspan',
            'handspan',
            'standingreach',
            'dominanthand']
        });


    var VueBio = new Vue({
        el: '#biology',
        data: {
            url_statistics: '/api/GetAthleteCurrentStatistic',
            url_performanceData: '/api/GetAthletePerformanceData',
            lists: [],
            statistics_loaded: false,
            diagram_loaded: false,
            canvas_height:0,
        },
        async mounted() {
            axios.get(this.url_statistics + '/' + profileMenu.user_id)
                .then(response => {
                    var str = response.data.toString();
                    if (str.indexOf("<!--This is the login layout-->") == 0) {
                        alert("The system detects that you have not operated for a long time, please login again");
                        document.clear();
                        location.reload();
                    }
                    else {
                        if (response.data == 'null') {
                            VueBio.lists.push({
                                id: 0,
                                height: '-',
                                weight: '-',
                                wingspan: '-',
                                handspan: '-',
                                standingreach: '-',
                                dominanthand: '-',
                            });
                        }
                        else {
                            var json = JSON.parse(response.data);
                            VueBio.lists.push({
                                id: 0,
                                height: (json.Height == 0 || json.Height == undefined) ? '-' : json.Height + ' cm',
                                weight: (json.Weight == 0 || json.Weight == undefined) ? '-' : json.Weight + ' kg',
                                wingspan: (json.Wingspan == 0 || json.Wingspan == undefined) ? '-' : json.Wingspan + ' cm',
                                handspan: (json.Handspan == 0 || json.Handspan == undefined) ? '-' : json.Handspan + ' cm',
                                standingreach: (json.StandingReach == 0 || json.StandingReach == undefined) ? '-' : json.StandingReach + ' cm',
                                dominanthand: (json.DominantHand == 0 || json.DominantHand == undefined) ? '-' : json.DominantHand
                            });
                        }
                        VueBio.statistics_loaded = true;
                    }
                }).catch(error => {
                    $("#body-content").html(error.response);
                });

            var cates = [];
            var data = [];

            await axios.get(this.url_performanceData + '/' + profileMenu.user_id)
                .then(response => {
                    VueBio.diagram_loaded = true;
                    var str = response.data.toString();
                    if (str.indexOf("<!--This is the login layout-->") == 0) {
                        alert("The system detects that you have not operated for a long time, please login again");
                        document.clear();
                        location.reload();
                    }
                    else {
                        if (response.data == undefined) {
                            return;
                        }
                        $.each(JSON.parse(response.data), function (index, item) {
                            cates.push(item.Category);
                            data.push(item.Score);
                        });
                    }
                }).catch(error => {
                    $("#body-content").html(error.response);
                });

            if (cates.length > 0 && data.length > 0) {
                VueBio.canvas_height = 980;
                new Chart(document.getElementById("performanceDiagram"),
                    {
                        "type": "radar", "data":
                        {
                            "labels": cates,
                            "datasets":
                                [{
                                    "label": "",
                                    "data": data,
                                    "fill": true, "backgroundColor": "rgba(255, 221, 0, 0.7)",
                                    "borderColor": "#ffdd00",
                                    "pointBackgroundColor": "rgb(255, 99, 132)",
                                    "pointBorderColor": "#fff",
                                    "pointHoverBackgroundColor": "#fff",
                                    "pointHoverBorderColor": "rgb(255, 99, 132)"
                                }]
                        },
                        "options":
                        {
                            "scale": {
                                "pointLabels": {
                                    "fontSize": 16,
                                    "fontColor": "#ffdd00",
                                },
                                gridLines: {
                                    color: ["none", "none", "none", "none", "none", "none", "none", "none", "none", "#b2b2b2"],
                                    lineWidth: 3
                                },
                                ticks: {
                                    display: false,
                                    maxTicksLimit: 1,
                                    suggestedMin: 0,
                                    suggestedMax: 120
                                }
                            },
                            "elements":
                            {
                                "line":
                                {
                                    "tension": 0,
                                    "borderWidth": 3
                                }
                            }
                        }
                    });
            }
        }
    });
    Vue.component('combine-records', {
        template: '\
                    <div class="row">\
                        <h3 class="mt-2 col-12 medium-bold-400">\
                            {{combine_name}}<span class="material-icons pl-3 pointer" data-toggle="tooltip" data-placement="right" :title="tests" v-on:mouseover="$emit(\'hover\',$event)">\
                                error_outline\
                            </span>\
                        </h3>\
                        <div class="col-4 text-center">\
                            <h3 class="text-FXV medium-bold-400 mt-1">{{bestattempt}}</h3>\
                            <p class="text-muted">Best Attempt</p>\
                        </div><div class="col-4 text-center">\
                            <h3 class="text-FXV medium-bold-400 mt-1">{{rank}}</h3>\
                            <p class="text-muted">Rank</p>\
                        </div><div class="col-4 text-center">\
                            <h3 class="text-FXV medium-bold-400 mt-1">{{topscore}}</h3>\
                            <p class="text-muted">Top Score</p>\
                        </div>\
                    </div>\
        ',
        props: ["combine_name",
            'tests',
            'bestattempt',
            'rank',
            'topscore']
    });

    var VueCombineRecords = new Vue({
        el: '#combine-records',
        data: {
            lists: [],
            NextToDoId: 0,
            combine_records_loaded: false,
            no_combine_record: false,
            url:'/api/GetAthleteCombineRecords',
        },
        async mounted() {

            await axios.get(this.url + '/' + profileMenu.user_id)
                .then(response => {
                    VueCombineRecords.combine_records_loaded = true;
                    var str = response.data.toString();
                    if (str.indexOf("<!--This is the login layout-->") == 0) {
                        alert("The system detects that you have not operated for a long time, please login again");
                        document.clear();
                        location.reload();
                    }
                    else {
                        var json = JSON.parse(response.data);
                        if (json.length == 0) {
                            VueCombineRecords.no_combine_record = true;
                        }
                        else {
                            $.each(json, function (index, item1) {

                                var tests = 'Tests: ';

                                $.each(item1.TestNames, function (index, item2) {
                                    tests += item2 + ', ';
                                });

                                tests = tests.substring(0, tests.length - 2);

                                VueCombineRecords.lists.push({
                                    id: VueCombineRecords.NextToDoId ++,
                                    combine_name: item1.CombineName,
                                    tests: tests,
                                    bestattempt: item1.BestAttemp,
                                    rank: item1.Rank,
                                    topscore: item1.TopScore
                                });

                            });
                        }
                    }
                }).catch(error => {
                    $("#body-content").html(error.response);
                });
        },
        methods: {
            ShowTests: function (event) {
                $('[data-toggle="tooltip"]').tooltip({ animation: true });
            }
        }
    });

    Vue.component('athlete_orgs', {
        template: '\
                <div class="row">\
                    <h3 class="mt-1 col-12 medium-bold-400 text-white">{{org}}</h3>\
                    <div class="col-4 text-center" v-for="t_list in teams">\
                        <div class="profile-team-img" v-bind:style="{ backgroundImage: `url(${t_list.imgpath})` }"></div>\
                            <p class="text-muted mt-1">{{t_list.teamname}}</p>\
                        </div >\
                    <p v-bind:class="{\'text-left text-muted ml-5\':true, \'display-none\': !hasteam}">No team has been assigned</p>\
                </div>\
        ',
        props: ['org', 'teams', 'hasteam']
    });

    var VueOrgsTeams = new Vue({
        el: '#athlete_orgs_teams',
        data: {
            url: '/api/GetAthleteOrgsTeams',
            orgs: [],
            NextOrgToDoId: 0,
            NextTeamToDoId: 0,
            data_loaded: false,
            has_org: true,
        },
        mounted() {
            axios.get(this.url + '/' + profileMenu.user_id)
                .then(response => {
                    VueOrgsTeams.data_loaded = true;
                    var str = response.data.toString();
                    if (str.indexOf("<!--This is the login layout-->") == 0) {
                        alert("The system detects that you have not operated for a long time, please login again");
                        document.clear();
                        location.reload();
                    }
                    else {
                        var json = JSON.parse(response.data);

                        if (json.length > 0) {
                            VueOrgsTeams.has_org = true;
                            $.each(json, function (index1, item1) {
                                var temps = [];
                                $.each(item1.Teams, function (index2, item2) {
                                    temps.push({
                                        id: VueOrgsTeams.NextTeamToDoId++,
                                        imgpath: item2.Img_Path,
                                        teamname: item2.Name
                                    });
                                });

                                VueOrgsTeams.orgs.push({
                                    id: VueOrgsTeams.NextOrgToDoId++,
                                    org: item1.Organization,
                                    teams: temps,
                                    hasteam: temps == 0
                                });
                                VueOrgsTeams.NextTeamToDoId = 0;
                            });
                        }
                        else {
                            VueOrgsTeams.has_org = false;
                        }
                    }
                })
                .catch(error => {
                    $("#body-content").html(error.response);
                });
        }
    });