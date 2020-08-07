
Vue.component('event-element', {
    template: '\
            <div class= "list-item" v-on:click="$emit(\'get_event_info\')">\
            <div class="row">\
                <div class="col-lg-2 col-md-3 col-0 list-item-img-block">\
                    <img v-bind:src="img_path">\
            </div>\
                    <div class="col-lg-8 offset-lg-0 col-md-7 offset-md-0 col-sm-8 offset-sm-1 col-12 offset-0 pb-2">\
                        <h3 class="text-FXV mt-2">{{eventname}}</h3>\
                        <div class="row text-white">\
                            <p class="col-12 mb-2">\
                                <span class="material-icons mr-1" style="float:left;line-height: 19px;font-size: 19px;">\
                                    location_on\
                        </span> {{location}}\
                    </p>\
                            <p class="col-xl-3 col-lg-4 col-md-6 col-sm-7 col-12">\
                                <span class="text-muted">\
                                    Date:\
                        </span> {{date}}\
                    </p>\
                            <p class="col-xl-3 col-lg-4 col-md-6 col-sm-5 col-12">\
                                <span class="text-muted">\
                                    Time:\
                        </span> {{time}}\
                    </p>\
                        </div>\
                    </div>\
                    <div class="col-md-2 col-sm-3 col-12 text-right medium-bold text-FXV list-item-block-sm-hide">\
                        <p class="text-muted mt-2"><span class="text-FXV font-x-large medium-bold-400">{{registered_num}}</span> Registered</p>\
                        <p v-bind:class="{\'font-weight-bold\': true, \'font-x-large\':true, \'text-event-pending\': pending, \'text-event-started\': started, \'text-event-closed\': closed }">{{status}}</p>\
                    </div>\
                    <div class="col-12 medium-bold text-FXV list-item-block-sm-show pb-4">\
                        <p v-bind:class="{\'font-weight-bold\': true, \'font-x-large\':true, \'text-event-pending\': pending, \'text-event-started\': started, \'text-event-closed\': closed }">{{status}}</p>\
                    </div>\
                </div>\
                <div v-bind:class="{\'list-item-extend\': true, \'list-item-extend-hide\': detail_hide, \'pl-5\':true, \'pr-5\':true}">\
                    <div class="border-top w-100 pb-2"></div>\
                    <div is="list-item-extend" v-for="detail in details" v-bind:key="detail.id" v-bind:combinename="detail.combinename" v-bind:tests="detail.tests"></div>\
                    <p class="loading m-auto text-center text-white pt-3 pointer pb-5" >Loading...<img style="width: 2rem; height: 2rem; margin-left:1rem" src="/sources/img/loading.gif" alt="loading..."/></p>\
                </div>\
                <span v-bind:class="{\'material-icons\':true, \'text-white\':true, \'list-item-arrow-down\': arrow_down, \'list-item-arrow-up\':arrow_up}" v-on:click="$emit(\'get_detail\', $event)" v-on:click.stop="">\
                  keyboard_arrow_down\
              </span>\
         </div>\
        ',
    props: ['img_path', 'eventname', 'location', 'date', 'time', 'status', 'registered_num', 'pending', 'started', 'closed', 'arrow_up', 'arrow_down', 'detail_hide', 'details'],
});

var vueEventList = new Vue({
    el: "#eventlist",
    data: {
        last_id: 0,
        num: 10,
        url_event_list: '/api/GetEventList',
        url_event_combine_detail: '/api/GetEventCombineDetail',
        url_event_info: '/EventInfo/EventInfo',
        lists: [],
        nextToDoId: 0,
        hasMore: true,
    },
    mounted() {
        this.LoadMore();
    },
    computed: {

    },
    methods: {
        Freshable: function () {
            return this.hasMore;
        },
        LoadMore: function () {
            if (this.hasMore) {
                $('#more').html('Loading...<img style="width: 2rem; height: 2rem; margin-left:1rem" src="/sources/img/loading.gif" alt=""/>');
                axios.get(this.url_event_list + "/" + this.num + "/" + this.last_id)
                    .then(response => {
                        var str = response.data.toString();
                        if (str.indexOf("<!--This is the login layout-->") == 0) {
                            alert("The system detects that you have not operated for a long time, please login again");
                            document.clear();
                            location.reload();
                        }
                        else {
                            var json = JSON.parse(response.data);

                            $.each(json, function (index, item) {
                                vueEventList.lists.push({
                                    id: vueEventList.nextToDoId++,
                                    eventid: item.EventId,
                                    eventname: item.Name,
                                    status: item.Status,
                                    img_path: item.Img_Path,
                                    location: item.Location,
                                    date: item.Date,
                                    time: item.Time,
                                    registered_num: item.Registered,
                                    combine_name: item.Combine_Name,
                                    pending: (item.Status == "Pending"),
                                    started: (item.Status == "Started"),
                                    closed: (item.Status == "Closed"),
                                    arrow_up: false,
                                    arrow_down: true,
                                    detail_hide: true,
                                    details: [],
                                });
                                vueEventList.last_id = (item.EventId > vueEventList.last_id) ? item.EventId : vueEventList.last_id;
                            });


                            if (json.length < 10) {
                                $('#more').attr('disabled', 'true').removeClass('text-white').addClass('text-muted').html("No more");
                                vueEventList.hasMore = false;
                            }
                            else {
                                $('#more').removeAttr('disabled').removeClass('text-muted').addClass('text-white').html("More...");
                                vueEventList.hasMore = true;
                            }
                        }
                    }).catch(error => {
                        $("#body-content").html(error.response);
                    })
            }
        },
        GetDetail: function (e_id, list, event) {

            if (list.arrow_down && !list.arrow_up) {

                $.each(vueEventList.lists, function (index, item) {
                    item.arrow_down = true;
                    item.arrow_up = false;
                    item.detail_hide = true;
                })

                list.arrow_down = !list.arrow_down;
                list.arrow_up = !list.arrow_up;
                list.detail_hide = !list.detail_hide;

                axios.get(this.url_event_combine_detail + '/' + e_id)
                    .then(response => {
                        var str = response.data.toString();
                        if (str.indexOf("<!--This is the login layout-->") == 0) {
                            alert("The system detects that you have not operated for a long time, please login again");
                            document.clear();
                            location.reload();
                        }
                        else {
                            var json = JSON.parse(response.data);
                            list.details.pop();
                            list.details.push({
                                id: 1,
                                tests: json.TestNames,
                                combinename: json.Combine_Name
                            });
                            $(event.target).parent().find('.list-item-extend').find('.loading').attr('hidden', 'hidden');
                        }
                    })
                    .catch(error => {
                        $("#body-content").html(error.response);
                    })
            }
            else if (!list.arrow_down && list.arrow_up) {
                list.arrow_down = !list.arrow_down;
                list.arrow_up = !list.arrow_up;
                list.detail_hide = !list.detail_hide;
                $(event.target).parent().find('.list-item-extend').find('.loading').removeAttr('hidden', 'hidden');
            }
        },
        GetEventInfo: function (e_id) {
            var params = new URLSearchParams();
            params.append('id', e_id);

            axios.get(this.url_event_info, {
                params: params
            })
                .then(response => {
                    var str = response.data.toString();
                    if (str.indexOf("<!--This is the login layout-->") == 0) {
                        alert("The system detects that you have not operated for a long time, please login again");
                        document.clear();
                        location.reload();
                    }
                    else {
                        $('#body-content').html(response.data);
                    }
                })
                .catch(error => {
                    $("#body-content").html(error.response);
                });
        }
    }
});

Vue.component('list-item-extend', {
    template: '\
        <div class="row pb-4">\
            <!--specification-->\
            <div class="col-12 text-center text-white ml-0 mr-0 pl-0 pr-0">\
            <div class="row col-12 text-left pb-2 ml-0 mr-0 pl-0 pr-0 text-center">\
               <p class="col-12 text-muted mt-4">Combine: <span class="text-FXV font-x-large font-weight-bold medium-bold-400">{{combinename}}</span></p>\
            </div>\
            </div>\
            <div class="col-12 text-center text-white pl-0 pr-0 pb-4">\
               <div class="row col-12 pr-0">\
                   <h3 class="col-12 text-white font-weight-bold mt-0">Tests</h3>\
                   <span v-if="tests.length == 0 || tests.length == 1" class="col-12 text-center text-muted">No Test</span>\
                   <span v-else-if="tests.length == 2" class="col-12 col-md-6" v-for="test in tests">{{test}}</span>\
                   <span v-else-if=" tests.length > 2 " class="col-12 col-md-6 col-lg-4" v-for="test in tests">{{test}}</span>\
               </div>\
            </div>\
         </div>\
        ',
    props: ['combinename', 'tests']
});

var vueEventMenu = new Vue({
    el: "#event-menu",
    data: {
        url_Search: '/api/GetEventsByName',
        url_Add: '/Events/Create',
        url_Search_By_Date: '/api/GetEventsByDate',
        SearchText: '',
        startDate: '',
        endDate: '',
    },
    mounted() {
        var date = new Date();

        this.endDate = moment(date, 'YYYY-MM-DD').format('YYYY-MM-DD');
        date.setMonth(date.getMonth() - 1);
        this.startDate = moment(date, 'YYYY-MM-DD').format('YYYY-MM-DD');
    },
    watch: {
        SearchText: 'CheckSearchText',
    },
    methods: {
        CheckSearchText: function () {
            this.SearchText = this.SearchText.toString().replace(/[\\/<>'"]/g, '');
        },
        AddEvent: function () {
            axios.get(this.url_Add)
                .then(response => {
                    var str = response.data.toString();
                    if (str.indexOf("<!--This is the login layout-->") == 0) {
                        alert("The system detects that you have not operated for a long time, please login again");
                        document.clear();
                        location.reload();
                    }
                    else {
                        $('#body-content').html(response.data);
                    }
                })
                .catch(error => {
                    $("#body-content").html(error.response);
                });
        },
        Search: _.debounce(
            function () {
                $('#more').html('Loading...<img style="width: 2rem; height: 2rem; margin-left:1rem" src="/sources/img/loading.gif" alt=""/>');
                vueEventList.lists = [];

                if (this.SearchText == "") {
                    vueEventList.last_id = 0;
                    vueEventList.hasMore = true;
                    vueEventList.LoadMore();
                }
                else {
                    this.SendSearchRequest();
                }
            }, 500),
        SendSearchRequest: function () {
            axios.get(this.url_Search + '/' + this.SearchText)
                .then(response => {
                    var str = response.data.toString();
                    if (str.indexOf("<!--This is the login layout-->") == 0) {
                        alert("The system detects that you have not operated for a long time, please login again");
                        document.clear();
                        location.reload();
                    }
                    else {
                        var json = JSON.parse(response.data);
                        $.each(json, function (index, item) {
                            vueEventList.lists.push({
                                id: vueEventList.nextToDoId++,
                                eventid: item.EventId,
                                eventname: item.Name,
                                status: item.Status,
                                img_path: item.Img_Path,
                                location: item.Location,
                                date: item.Date,
                                time: item.Time,
                                registered_num: item.Registered,
                                combine_name: item.Combine_Name,
                                pending: (item.Status == "Pending"),
                                started: (item.Status == "Started"),
                                closed: (item.Status == "Closed"),
                                arrow_up: false,
                                arrow_down: true,
                                detail_hide: true,
                                details: [],
                            });
                        });


                        $('#more').attr('disabled', 'true').removeClass('text-white').addClass('text-muted').html("No more");
                        vueEventList.hasMore = false;
                    }
                }).catch(error => {
                    $("#body-content").html(error.response);
                })
        },
        SearchByDate: function () {
            $('#more').html('Loading...<img style="width: 2rem; height: 2rem; margin-left:1rem" src="/sources/img/loading.gif" alt=""/>');
            vueEventList.lists = [];

            axios.get(this.url_Search_By_Date + '/' + this.startDate + '/' + this.endDate)
                .then(response => {
                    var str = response.data.toString();
                    if (str.indexOf("<!--This is the login layout-->") == 0) {
                        alert("The system detects that you have not operated for a long time, please login again");
                        document.clear();
                        location.reload();
                    }
                    else {
                        var json = JSON.parse(response.data);
                        $.each(json, function (index, item) {
                            vueEventList.lists.push({
                                id: vueEventList.nextToDoId++,
                                eventid: item.EventId,
                                eventname: item.Name,
                                status: item.Status,
                                img_path: item.Img_Path,
                                location: item.Location,
                                date: item.Date,
                                time: item.Time,
                                registered_num: item.Registered,
                                combine_name: item.Combine_Name,
                                pending: (item.Status == "Pending"),
                                started: (item.Status == "Started"),
                                closed: (item.Status == "Closed"),
                                arrow_up: false,
                                arrow_down: true,
                                detail_hide: true,
                                details: [],
                            });
                        });

                        if (json.length > 0) {
                            $('#more').attr('disabled', 'true').removeClass('text-white').addClass('text-muted').html("No more");
                        }
                        else if (json.length == 0) {
                            $('#more').attr('disabled', 'true').removeClass('text-white').addClass('text-muted').html("No record between " + vueEventMenu.startDate + " and " + vueEventMenu.endDate);
                        }
                        vueEventList.hasMore = false;
                    }
                }).catch(error => {
                    $("#body-content").html(error.response);
                })
        },
    }
});