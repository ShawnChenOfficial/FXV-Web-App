var deleteTestPopUp = new Vue({
    el: '#deleteConfirmPopUp',
    data: {
        Deleting: false,
        Test_Id: 0,
        event: null,
        url_remove: '/Tests/Remove',
    },
    methods: {
        DeleteTestConfirmed: function () {
            $('#loading-panel').removeAttr('hidden');

            var params = new URLSearchParams();
            params.append('test_id', this.Test_Id);

            axios.post(this.url_remove, null, {
                params: params
            }).then(response => {
                var str = response.data.toString();
                if (str.indexOf("<!--This is the login layout-->") == 0) {
                    alert("The system detects that you have not operated for a long time, please login again");
                    document.clear();
                    location.reload();
                }
                else {
                    if (response.data.Success) {
                        vueTestMenu.Search();
                        SuccessTurnToParentPage("Success", true);
                    }
                    else {
                        SuccessTurnToParentPage(response.data.Reason, false);
                    }
                }
            }).catch(error => {
                $("#body-content").html(error.response);
            });

            deleteTestPopUp.Deleting = false;
            $('#loading-panel').attr('hidden', 'hidden');
        },
        DeleteCancelled: function () {
            deleteTestPopUp.Deleting = false;
        }
    },
    computed: {
        Show: function () {
            if (!this.Deleting) {
                this.Test_Id = 0;
                this.event = null;
            }
            return this.Deleting;
        }
    }
});

Vue.component('test-list-item', {
    template: '\
        <div class="w-100 display-inline-flex">\
          <div class="list-item w-100">\
              <div class="row ml-4 mr-4">\
                  <div class="col-md-10 col-sm-9 col-12 pb-2">\
                  <h3 class="text-FXV mt-2">{{name}} <span v-bind:class="gender"></span></h3>\
                  <p class="text-muted">{{description}}</p>\
                  </div>\
                  <div class="col-md-2 col-3 text-right medium-bold text-FXV list-item-category-block pl-0">\
                  <p class="text-muted mt-2">Tested <span class="text-white font-x-large medium-bold-400">{{tested}}</span></p>\
                  <h2 class="font-weight-bold mb-1">{{cate_abbri}}</h2>\
                  </div>\
              </div>\
              <div v-bind:class="{\'list-item-extend\': true, \'list-item-extend-hide\': detail_hide, \'pl-5\':true, \'pr-5\':true}">\
                  <div is="test-list-item-extend" v-for="detail in details" v-bind:key = "detail.id" v-bind:sys_per="detail.sys_per" v-bind:test_id="detail.test_id" v-bind:cate_full="detail.cate_full" v-bind:unit="detail.unit" v-bind:promiex_score="detail.promiex_score"\
                  v-bind:promiex_profile_img="detail.promiex_profile_img" v-bind:promiex_name="detail.promiex_name" v-bind:promiex_gender="detail.promiex_gender" v-bind:promiex_age="detail.promiex_age" v-bind:promiex_result="detail.promiex_result"\
                  v-bind:top_score="detail.top_score" v-bind:top_profile_img="detail.top_profile_img" v-bind:top_name="detail.top_name" v-bind:top_gender="detail.top_gender" v-bind:top_age="detail.top_age" v-bind:top_result="detail.top_result"\
                  v-bind:bronze_score="detail.bronze_score" v-bind:bronze_profile_img="detail.bronze_profile_img" v-bind:bronze_name="detail.bronze_name" v-bind:bronze_gender="detail.bronze_gender" v-bind:bronze_age="detail.bronze_age" v-bind:bronze_result="detail.bronze_result"\
                  v-bind:activities="detail.activities" v-on:edit_test="EditTest(test_id, $event)" v-on:delete_test="DeleteTest(test_id, $event)" ></div>\
                  <p class="loading m-auto text-center text-white pt-3 pointer pb-5" >Loading...<img style="width: 2rem; height: 2rem; margin-left:1rem" src="/sources/img/loading.gif" alt="" /></p>\
              </div>\
            <span v-bind:class="{\'material-icons\':true, \'text-white\':true, \'list-item-arrow-down\': arrow_down, \'list-item-arrow-up\':arrow_up}" v-on:click="$emit(\'get_detail\', $event)">\
                  keyboard_arrow_down\
              </span>\
          </div>\
          <div v-if="!hasrunningactivity" class="btn run-button" @click="RunActivity">\
            <p>Run</p>\
          </div>\
          <div v-if="hasrunningactivity" class="btn run-button-running" @click="BackToActivity">\
            <p>Running</p>\
          </div>\
        </div>\
      ',
    props: ['test_id', 'img_path', 'name', 'gender', 'description', 'tested', 'cate_abbri', 'arrow_down', 'arrow_up', 'detail_hide', 'details', 'hasrunningactivity'],
    methods: {
        EditTest: function (test_id, event) {
            $('#loading-panel').removeAttr('hidden');
            window.location.href = "/Tests/Edit?test_id=" + test_id;
            $('#loading-panel').attr('hidden', 'hidden');
        },
        DeleteTest: function (test_id, event) {
            deleteTestPopUp.Deleting = true;
            deleteTestPopUp.Test_Id = test_id;
            deleteTestPopUp.event = event;
        },
        RunActivity() {
            window.location.href = '/Activity/Index?testid=' + this.test_id;
        },
        BackToActivity() {
            alert("Test Acticity is coming soon..");
        }
    }
});

Vue.component('test-list-item-extend', {
    template: '\
     <div class="row">\
        <div class="border-top w-100 pb-2"></div>\
            <div v-if="sys_per === \'Admin\'" class="col-12 offset-0 col-md-5 offset-md-7 col-lg-4 offset-lg-8 col-xl-3 offset-xl-9 pb-2 text-center text-white p-0">\
                <a v-on:click="$emit(\'edit_test\', $event)" class="pointer">\
                    <span class="material-icons pr-1">\
                        create\
                    </span>Edit\
                </a>\
                <span class="text-muted font-2rem ml-2 mr-2 menu-divider-vertical">|</span>\
                <a v-on:click="$emit(\'delete_test\', $event)" class="pointer">\
                    <span class="material-icons pr-1">\
                        delete\
                    </span>Delete\
                </a>\
            </div>\
        <div class="col-12 pb-4 text-center text-white">\
            <div class="row col-12 offset-0 col-md-10 offset-md-1 col-lg-8 offset-lg-2 col-xl-6 offset-xl-3">\
                <p class="col-6 text-muted">Category <span class="text-white font-x-large pl-2">{{cate_full}}</span></p>\
                <p class="col-6 text-muted">Mesurement <span class="text-white font-x-large pl-2">{{unit}}</span></p>\
            </div>\
        </div>\
        <!--rank-->\
        <div class="col-12 pb-5 col-md-show container-fluid row p-0 m-0">\
            <div class="text-center col-md-12 offset-md-0 col-lg-10 offset-lg-1 col-xl-8 offset-xl-2 row p-0">\
                <div class="col-4 p-0">\
                    <div class="promiex pl-3 pr-3 mt-3 text-FXV">\
                        <h4 class="font-weight-bold mt-1">Second</h4>\
                        <h4 class="medium-bold promiex-score mt-1">{{promiex_score}}</h4>\
                        <div class="promiex-profile-img" :style="{ backgroundImage: `url(${promiex_profile_img})` }"></div>\
                        <h5>{{promiex_name}}</h5>\
                        <div class="display-inline-flex">\
                            <span v-bind:class="promiex_gender"></span>\
                            <h5 class="pl-2">{{promiex_age}}</h5>\
                        </div>\
                        <h5>{{promiex_result}} <small>{{unit}}</small></h5>\
                    </div>\
                </div>\
                <div class="col-4 p-0">\
                    <div class="topAthlete pl-3 pr-3 text-FXV">\
                        <h3 class="font-weight-bold mt-2">FXV Record</h3>\
                        <h3 class="medium-bold topAthlete-score mt-1">{{top_score}}</h3>\
                        <div class="topAthlete-profile-img" :style="{ backgroundImage: `url(${top_profile_img})` }"></div>\
                        <h4>{{top_name}}</h4>\
                        <div class="display-inline-flex">\
                            <span v-bind:class="top_gender"></span>\
                            <h4 class="pl-2">{{top_age}}</h4>\
                        </div>\
                        <h4>{{top_result}} <small>{{unit}}</small></h4>\
                    </div>\
                </div>\
                <div class="col-4 p-0">\
                    <div class="bronze pl-3 pr-3 mt-3 text-FXV">\
                        <h4 class="font-weight-bold mt-1">Third</h4>\
                        <h4 class="medium-bold bronze-score mt-1">{{bronze_score}}</h4>\
                        <div class="bronze-profile-img" :style="{ backgroundImage: `url(${bronze_profile_img})` }"></div>\
                        <h5>{{bronze_name}}</h5>\
                        <div class="display-inline-flex">\
                            <span v-bind:class="bronze_gender"></span>\
                            <h5 class="pl-2">{{bronze_age}}</h5>\
                        </div>\
                        <h5>{{bronze_result}} <small>{{unit}}</small></h5>\
                    </div>\
                </div>\
            </div>\
            <div class="col-12 text-center pointer mt-3">\
                <a class= "text-FXV" href="/Leaderboards/Leaderboards">Click for more information</a>\
            </div>\
        </div>\
        <div class="col-12 pb-5 col-md-show container-fluid p-0 m-0">\
            <div class="border-top w-100 pb-2"></div>\
            <div is="current_activities" v-for="activity in activities" v-bind:key="activity.id" v-bind:weekday="activity.weekday"\
                    v-bind:date="activity.date" v-bind:month="activity.month" v-bind:time="activity.time" v-bind:testid="activity.testid">\
            </div>\
        </div>\
        <div class="col-12 text-center pb-2 text-muted col-md-hide">\
            <p>Further information is only available for the larger screen size.</p>\
        </div>\
    </div>\
        ',
    props: [
        'sys_per',
        'cate_full',
        'unit',
        'promiex_score',
        'promiex_profile_img',
        'promiex_name',
        'promiex_gender',
        'promiex_age',
        'promiex_result',
        'top_score',
        'top_profile_img',
        'top_name',
        'top_gender',
        'top_age',
        'top_result',
        'bronze_score',
        'bronze_profile_img',
        'bronze_name',
        'bronze_gender',
        'bronze_age',
        'bronze_result',
        'activities'
    ]
});


Vue.component('current_activities', {
    template: '\
    <div class="row text-center text-white">\
                <p class="col-4">{{weekday}} {{date}} {{month}}</p>\
                <p class="col-2">{{time}}</p>\
                <a class="col-3 pointer" v-on:click="ReRun"><span class="material-icons">replay</span>Re-Run Test</a>\
                <a class="col-3 pointer" v-on:click="ShowResults"><span class="material-icons">insert_chart</span>Show Results</a>\
    </div>\
    \
',
    props: ['weekday', 'date', 'month', 'time', 'testid'],
    methods: {
        ReRun() {
            alert("The function of Re-run activity is comming. Test ID: " + this.testid);
        },
        ShowResults() {
            alert("The function of Show Results is comming. Test ID: " + this.testid);
        }
    }
});

var vueTestMenu = new Vue({
    el: "#test-menu",
    data() {
        return {
            url_Search: '/api/GetTestsByName',
            url_Add: '/Tests/CreateTests',
            SearchText: '',
            searchByGender: false,
            isMale: false
        }
    },
    watch: {
        SearchText: 'CheckSearchText',
    },
    methods: {
        CheckSearchText: function () {
            this.SearchText = this.SearchText.toString().replace(/[\\/<>'"]/g, '');
        },
        Search: _.debounce(
            function () {
                $('#more').html('Loading...<img style="width: 2rem; height: 2rem; margin-left:1rem" src="/sources/img/loading.gif" alt=""/>');
                vueTestList.lists = [];

                if (this.SearchText == "") {
                    vueTestList.last_id = 0;
                    vueTestList.hasMore = true;
                    vueTestList.LoadMore();
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
                            vueTestList.lists.push({
                                id: vueTestList.nextTodoId++,
                                name: item.Name,
                                description: item.Description,
                                tested: item.Tested,
                                cate_abbri: item.Category[0],
                                arrow_up: false,
                                arrow_down: true,
                                detail_hide: true,
                                hasrunningactivity: item.Status != 0,
                                test_id: item.TestId,
                                details: [],
                                gender: (item.Gender.toString() == 'Male') ? 'male-icon-large pl-2' : (item.Gender.toString() == 'Female') ? 'female-icon-large pl-2' : ''
                            })
                        });
                        $('#more').attr('disabled', 'true').removeClass('text-white').addClass('text-muted').html("No more");
                        vueTestList.hasMore = false;
                    }
                }).catch(error => {
                    $("#body-content").html(error.response);
                })
        },
        GetOnlyMale() {
            alert("Function 'Get Only Male' is coming.");
        },
        GetOnlyFemale() {
            alert("Function 'Get Only Female' is coming.");
        },
    }
});

var vueTestList = new Vue({
    el: "#list",
    data: {
        last_id: 0,
        url_test_list: '/api/GetTestList',
        url_test_detail: '/api/GetTestDetail',
        hasMore: true,
        lists: [],
        nextTodoId: 0
    },
    mounted() {
        this.LoadMore();
    },
    computed: {

    },
    methods: {
        LoadMore: function () {
            if (this.hasMore) {
                $('#more').html('Loading...<img style="width: 2rem; height: 2rem; margin-left:1rem" src="/sources/img/loading.gif" alt=""/>');
                axios.get(this.url_test_list + "/" + this.last_id)
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

                                vueTestList.lists.push({
                                    id: vueTestList.nextTodoId++,
                                    name: item.Name,
                                    description: item.Description,
                                    tested: item.Tested,
                                    cate_abbri: item.Category[0],
                                    arrow_up: false,
                                    arrow_down: true,
                                    detail_hide: true,
                                    test_id: item.TestId,
                                    hasrunningactivity: item.Status != 0,
                                    details: [],
                                    gender: (item.Gender.toString() == 'Male') ? 'male-icon-large pl-2' : (item.Gender.toString() == 'Female') ? 'female-icon-large pl-2' : ''
                                })

                                vueTestList.last_id = (item.TestId > vueTestList.last_id) ? item.TestId : vueTestList.last_id;
                            });

                            $('#more').attr('disabled', 'true').removeClass('text-white').addClass('text-muted').html("No more");
                            vueTestList.hasMore = false;
                        }
                    }).catch(error => {
                        $("#body-content").html(error.response);
                    })
            }
        },
        GetDetail: function (test_id, list, event) {

            if (list.arrow_down && !list.arrow_up) {

                $.each(vueTestList.lists, function (index, item) {
                    item.arrow_down = true;
                    item.arrow_up = false;
                    item.detail_hide = true;
                })

                list.arrow_down = !list.arrow_down;
                list.arrow_up = !list.arrow_up;
                list.detail_hide = !list.detail_hide;

                axios.get(this.url_test_detail + '/' + test_id)
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
                                sys_per: json.Sys_Permission,
                                unit: json.Unit,
                                cate_full: json.Category,
                                promiex_score: json.Promiex.Score,
                                promiex_profile_img: json.Promiex.ImgPath,
                                promiex_name: json.Promiex.AthleteName.substring(0, 10) + '...',
                                promiex_gender: (json.Promiex.Gender == "Male") ? 'male-icon' : (json.Promiex.Gender == "Female") ? 'female-icon' : '',
                                promiex_age: json.Promiex.Age,
                                promiex_result: json.Promiex.Result,
                                top_score: json.Top.Score,
                                top_profile_img: json.Top.ImgPath,
                                top_name: json.Top.AthleteName.substring(0, 10) + '...',
                                top_gender: (json.Promiex.Gender == "Male") ? 'male-icon' : (json.Promiex.Gender == "Female") ? 'female-icon' : '',
                                top_age: json.Top.Age,
                                top_result: json.Top.Result,
                                bronze_score: json.Bronze.Score,
                                bronze_profile_img: json.Bronze.ImgPath,
                                bronze_name: json.Bronze.AthleteName.substring(0, 10) + '...',
                                bronze_gender: (json.Promiex.Gender == "Male") ? 'male-icon' : (json.Promiex.Gender == "Female") ? 'female-icon' : '',
                                bronze_age: json.Bronze.Age,
                                bronze_result: json.Bronze.Result,
                                activities: [{ id: 1, testid: test_id, weekday: "Monday", date: "24th", month: "October", time: "6:15PM" }, { id: 2, testid: test_id, weekday: "Thursday", date: "25th", month: "October", time: "6:15AM" }]
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
    },
});