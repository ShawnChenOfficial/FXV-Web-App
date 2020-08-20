var deleteCombinePopUp = new Vue({
    el: '#deleteConfirmPopUp',
    data: {
        Deleting: false,
        Combine_Id: 0,
        event: null,
        url_remove: '/Combines/RemoveCombine',
    },
    methods: {
        DeleteCombineConfirmed: function () {
            $('#loading-panel').removeAttr('hidden');

            var params = new URLSearchParams();
            params.append('c_id', this.Combine_Id);

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
                        vueCombineMenu.Search();
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
            deleteCombinePopUp.Deleting = false;
        }
    },
    computed: {
        Show: function () {
            if (!this.Deleting) {
                this.Combine_Id = 0;
                this.event = null;
            }
            return this.Deleting;
        }
    }
});

Vue.component('combine-list-item', {
    template: '\
          <div class="list-item">\
              <div class="row">\
                  <div class="col-lg-2 col-md-3 col-0 list-item-img-block">\
                  <img v-bind:src="img_path" />\
                  </div>\
                  <div class="col-lg-8 offset-lg-0 col-md-7 offset-md-0 col-sm-8 offset-sm-1 col-12 offset-0 pb-2">\
                  <h3 class="text-FXV mt-2">{{name}} <span v-bind:class="gender"></span></h3>\
                  <p class="text-muted">{{description}}</p>\
                  </div>\
                  <div class="col-md-2 col-3 text-right medium-bold text-FXV list-item-category-block pl-0">\
                  <p class="text-muted mt-2">Tested <span class="text-white font-x-large medium-bold-400">{{tested}}</span></p>\
                  </div>\
              </div>\
              <div v-bind:class="{\'list-item-extend\': true, \'list-item-extend-hide\': detail_hide, \'pl-5\':true, \'pr-5\':true}">\
                  <div is="combine-list-item-extend" v-for="detail in details" v-bind:key="detail.id" v-bind:tests="detail.tests" v-bind:sys_per="detail.sys_per" v-bind:combine_id="detail.combine_id" v-bind:cate_full="detail.cate_full" v-bind:promiex_score="detail.promiex_score"\
                  v-bind:promiex_profile_img="detail.promiex_profile_img" v-bind:promiex_name="detail.promiex_name" v-bind:promiex_gender="detail.promiex_gender" v-bind:promiex_age="detail.promiex_age"\
                  v-bind:top_score="detail.top_score" v-bind:top_profile_img="detail.top_profile_img" v-bind:combineid="detail.combineid" v-bind:top_name="detail.top_name" v-bind:top_gender="detail.top_gender" v-bind:top_age="detail.top_age"\
                  v-bind:bronze_score="detail.bronze_score" v-bind:bronze_profile_img="detail.bronze_profile_img" v-bind:bronze_name="detail.bronze_name" v-bind:bronze_gender="detail.bronze_gender" v-bind:bronze_age="detail.bronze_age"\
                   v-on:delete_combine="DeleteCombine(combine_id, $event)" ></div>\
                  <p class="loading m-auto text-center text-white pt-3 pointer pb-5" >Loading...<img style="width: 2rem; height: 2rem; margin-left:1rem" src="/sources/img/loading.gif" alt="" /></p>\
              </div>\
            <span v-bind:class="{\'material-icons\':true, \'text-white\':true, \'list-item-arrow-down\': arrow_down, \'list-item-arrow-up\':arrow_up}" v-on:click="$emit(\'get_detail\', $event)">\
                  keyboard_arrow_down\
              </span>\
          </div>\
      ',
    props: ['combine_id', 'img_path', 'name', 'description', 'gender', 'tested', 'arrow_down', 'arrow_up', 'detail_hide', 'details'],
    methods: {
        DeleteCombine: function (combine_id, event) {
            deleteCombinePopUp.Deleting = true;
            deleteCombinePopUp.Combine_Id = combine_id;
            deleteCombinePopUp.event = event;
        },
    }
});


Vue.component('combine-list-item-extend', {
    template: '\
        <div class="row">\
        <div class="border-top w-100 pb-2"></div>\
            <div v-if="sys_per === \'Admin\'" class="col-12 offset-0 col-md-5 offset-md-7 col-lg-3 offset-lg-9 pb-2 col-xl-2 offset-xl-10 text-center text-white p-0">\
                <a v-bind:href="\'/Combines/Edit?c_id=\' + combineid" class="pointer">\
                    <span class="material-icons pr-1">\
                        create\
                    </span>Edit\
                </a>\
                <span class="text-muted font-2rem ml-2 mr-2 menu-divider-vertical">|</span>\
                <a v-on:click="$emit(\'delete_combine\', $event)" class="pointer">\
                    <span class="material-icons pr-1">\
                        delete\
                    </span>Delete\
                </a>\
            </div>\
        <div class="col-12 pb-4 text-center text-white pl-0 pr-0">\
            <div class="row col-12 pr-0">\
                <h3 class="col-12 text-FXV font-weight-bold mt-0">Tests</h3>\
                <span v-if="tests.length == 0 || tests.length == 1" class="col-12 text-center text-muted">No Test</span>\
                <span v-else-if="tests.length == 2" class="col-12 col-md-6" v-for="test in tests">{{test}}</span>\
                <span v-else-if=" tests.length > 2 " class="col-12 col-md-6 col-lg-4" v-for="test in tests">{{test}}</span>\
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
                    </div>\
                </div>\
            </div>\
        </div>\
        <div class="col-12 text-center pointer pb-5 col-md-show">\
        <a class= "text-FXV" href="/Leaderboards/Leaderboards">Click for more information</a>\
        </div>\
        <div class="col-12 text-center pb-5 text-muted col-md-hide">\
            <p>Further information is only available for the larger screen size.</p>\
        </div>\
    </div>\
        ',
    props: [
        'combineid',
        'sys_per',
        'tests',
        'promiex_score',
        'promiex_profile_img',
        'promiex_name',
        'promiex_gender',
        'promiex_age',
        'top_score',
        'top_profile_img',
        'top_name',
        'top_gender',
        'top_age',
        'bronze_score',
        'bronze_profile_img',
        'bronze_name',
        'bronze_gender',
        'bronze_age']
});
var vueCombineMenu = new Vue({
    el: "#combine-menu",
    data: {
        url_Search: '/api/GetCombinesByName',
        url_Add: '/Combines/CreateCombines',
        SearchText: '',
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
                vueCombineList.lists = [];

                if (this.SearchText == "") {
                    vueCombineList.last_id = 0;
                    vueCombineList.hasMore = true;
                    vueCombineList.LoadMore();
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

                            vueCombineList.lists.push({
                                id: vueCombineList.nextTodoId++,
                                name: item.Name,
                                description: item.Description,
                                tested: item.Tested,
                                img_path: item.Img_Path,
                                gender: (item.Gender.toString() == 'Male') ? 'male-icon-large pl-2' : (item.Gender.toString() == 'Female') ? 'female-icon-large pl-2' : '',
                                arrow_up: false,
                                arrow_down: true,
                                detail_hide: true,
                                combine_id: item.CombineId,
                                details: []
                            })
                        });

                        $('#more').attr('disabled', 'true').removeClass('text-white').addClass('text-muted').html("No more");
                        vueCombineList.hasMore = false;
                    }
                }).catch(error => {
                    $("#body-content").html(error.response);
                })
        }
    }
});

var vueCombineList = new Vue({
    el: "#list",
    data: {
        last_id: 0,
        num: 10,
        url_combine_list: '/api/GetCombineList',
        url_combine_detail: '/api/GetCombineDetail',
        hasMore: true,
        lists: [],
        nextTodoId: 0,
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
                axios.get(this.url_combine_list + "/" + this.num + "/" + this.last_id)
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
                                vueCombineList.lists.push({
                                    id: vueCombineList.nextTodoId++,
                                    name: item.Name,
                                    description: item.Description,
                                    tested: item.Tested,
                                    img_path: item.Img_Path,
                                    gender: (item.Gender.toString() == 'Male') ? 'male-icon-large pl-2' : (item.Gender.toString() == 'Female') ? 'female-icon-large pl-2' : '',
                                    arrow_up: false,
                                    arrow_down: true,
                                    detail_hide: true,
                                    combine_id: item.CombineId,
                                    details: [],
                                })
                                vueCombineList.last_id = (item.CombineId > vueCombineList.last_id) ? item.CombineId : vueCombineList.last_id;
                            });

                            if (json.length < 10) {
                                $('#more').attr('disabled', 'true').removeClass('text-white').addClass('text-muted').html("No more");
                                vueCombineList.hasMore = false;
                            }
                            else {
                                $('#more').removeAttr('disabled').removeClass('text-muted').addClass('text-white').html("More...");
                                vueCombineList.hasMore = true;
                            }
                        }
                    }).catch(error => {
                        $("#body-content").html(error.response);
                    })
            }
        },
        GetDetail: function (c_id, list, event) {

            if (list.arrow_down && !list.arrow_up) {

                $.each(vueCombineList.lists, function (index, item) {
                    item.arrow_down = true;
                    item.arrow_up = false;
                    item.detail_hide = true;
                })

                list.arrow_down = !list.arrow_down;
                list.arrow_up = !list.arrow_up;
                list.detail_hide = !list.detail_hide;


                axios.get(this.url_combine_detail + '/' + c_id)
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
                                combineid: json.CombineId,
                                tests: json.TestNames,
                                sys_per: json.Sys_Permission,
                                promiex_score: json.Promiex.Score,
                                promiex_profile_img: json.Promiex.ImgPath,
                                promiex_name: json.Promiex.AthleteName.substring(0, 10) + '...',
                                promiex_gender: (json.Promiex.Gender == "Male") ? 'male-icon' : (json.Promiex.Gender == "Female") ? 'female-icon' : '',
                                promiex_age: json.Promiex.Age,
                                top_score: json.Top.Score,
                                top_profile_img: json.Top.ImgPath,
                                top_name: json.Top.AthleteName.substring(0, 10) + '...',
                                top_gender: (json.Promiex.Gender == "Male") ? 'male-icon' : (json.Promiex.Gender == "Female") ? 'female-icon' : '',
                                top_age: json.Top.Age,
                                bronze_score: json.Bronze.Score,
                                bronze_profile_img: json.Bronze.ImgPath,
                                bronze_name: json.Bronze.AthleteName.substring(0, 10) + '...',
                                bronze_gender: (json.Promiex.Gender == "Male") ? 'male-icon' : (json.Promiex.Gender == "Female") ? 'female-icon' : '',
                                bronze_age: json.Bronze.Age
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
