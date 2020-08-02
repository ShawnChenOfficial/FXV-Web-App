
var deleteTeamPopUp = new Vue({
    el: '#deleteTeamPopUp',
    data: {
        Deleting: false,
        DeleteResult: false,
        url_delete: '/OrganizationTeamInfo/Remove',
    },
    methods: {
        DeleteTeamConfirmed: function () {
            $('#loading-panel').removeAttr('hidden');

            var params = new URLSearchParams();
            params.append('id', team_menu.team_id);

            axios.post(this.url_delete,null, {
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
                        DeleteResult = true;
                        this.RedirectToOrgInfo();
                        SuccessTurnToParentPage("Success", true);
                    }
                    else {
                        SuccessTurnToParentPage(response.data.Reason, false);
                    }
                }
            }).catch(error => {
                $("#body-content").html(error.response);
            });

            deleteTeamPopUp.Deleting = false;
            $('#loading-panel').attr('hidden', 'hidden');
        },
        DeleteCancelled: function () {
            deleteTeamPopUp.Deleting = false;
        },
        RedirectToOrgInfo: function () {
            if (DeleteResult) {

                var params = new URLSearchParams();
                params.append('id', team_menu.org_id);

                axios.get('/OrganizationInfo/OrganizationInfo', {
                    params: params
                }).then(response => {
                        var str = response.data.toString();
                        if (str.indexOf("<!--This is the login layout-->") == 0) {
                            alert("The system detects that you have not operated for a long time, please login again");
                            document.clear();
                            location.reload();
                        }
                        else {
                            $("#body-content").html(response.data);
                        }
                    })
                    .catch(error => {
                        $("#body-content").html(error.response);
                    });

                DeleteResult = false;
            }
        },
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

var team_menu = new Vue({
    el: '#team-menu',
    data: {
        team_id: '',
        org_id: '',
        url_team_edit:'/OrganizationTeamInfo/Edit'
    },
    mounted() {
        this.team_id = $('#team-menu').attr('data-team-id');
        this.org_id = $('#team-menu').attr('data-org-id');
    },
    methods: {
        DeleteTeam: function () {
            deleteTeamPopUp.Deleting = true;
        },
        EditTeam: function () {
            $('#loading-panel').removeAttr('hidden');

            var params = new URLSearchParams();
            params.append('team_id', this.team_id);

            axios.get(this.url_team_edit, {
                params: params
            }).then(response => {
                var str = response.data.toString();
                if (str.indexOf("<!--This is the login layout-->") == 0) {
                    alert("The system detects that you have not operated for a long time, please login again");
                    document.clear();
                    location.reload();
                }
                else {
                    if (response.data.Result != undefined && !response.data.Result && response.data.Reason != undefined) {
                        SuccessTurnToParentPage(response.data.Reason, false);
                    }
                    else {
                        $("#body-content").html(response.data);
                    }
                }
            }).catch(error => {
                $("#body-content").html(error.response);
            });

            $('#loading-panel').attr('hidden', 'hidden');
        },
    },
});

Vue.component('member-list-item', {
    template: '\
    <div class="list-item">\
        <div class="row pointer" v-on:click="$emit(\'get_profile\')">\
            <div class="col-lg-2 col-md-3 col-sm-2 list-item-img-block">\
                <img v-bind:src="img_path" />\
            </div>\
            <div class="col-lg-10 offset-lg-0 col-md-9 offset-md-0 col-12 offset-0 pb-2">\
                <h4 class="text-FXV mt-2">{{name}} <span v-bind:class="gender"></span> <span>{{age}}</span></h4>\
                <div class="row text-white pt-2">\
                    <p class="col-12 mb-1">\
                        <span class="material-icons mr-1 mb-1">\
                            location_on\
                        </span> {{location}}\
                    </p>\
                    <p class="col-xl-3 col-lg-4 col-md-5 col-12 text-left mb-1">\
                        <span class="text-muted">\
                            Role:\
                        </span> {{position}}\
                    </p>\
                </div>\
            </div>\
        </div>\
    </div>\
      ',
    props: ['img_path', 'name', 'gender', 'age', 'position', 'location', 'uid'],
});

var vueMembersMenu = new Vue({
    el: "#members-menu",
    data: {
        url_Search: '/api/GetUsersByTeamId_UserName',
        url_add_member: '/OrganizationTeamInfo/AddTeamMember',
        SearchText: '',
    },
    watch: {
        SearchText: 'CheckSearchText'
    },
    methods: {
        CheckSearchText: function () {
            this.SearchText = this.SearchText.toString().replace(/[\\/<>'"]/g, '');
        },
        Search: _.debounce(
            function () {
                $('#members-list').find('#more').html('Loading...<img style="width: 2rem; height: 2rem; margin-left:1rem" src="/sources/img/loading.gif" alt=""/>');
                vueMembersList.lists = [];

                if ($('#members-menu input').val() == "") {
                    vueMembersList.last_id = 0;
                    vueMembersList.hasMore = true;
                    vueMembersList.LoadMore();
                }
                else {
                    this.SendSearchRequest();
                }
            }, 500),
        SendSearchRequest: function () {
            axios.get(this.url_Search + '/' + team_menu.team_id + '/' + this.SearchText)
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
                            $.each(json, function (index, item) {
                                vueMembersList.lists.push({
                                    id: vueTeamsList.nextTodoId++,
                                    uid: item.UId,
                                    img_path: item.Img_Path,
                                    name: item.Name,
                                    age: item.Age,
                                    gender: (item.Gender.toString() == 'Male') ? 'male-icon-large pl-2' : (item.Gender.toString() == 'Female') ? 'female-icon-large pl-2' : '',
                                    position: item.Position,
                                    location: item.Location
                                });
                            });
                            $('#members-list').find('#more').attr('disabled', 'true').removeClass('text-white').addClass('text-muted').html("No more");
                        }
                        catch (e) {
                            $("#body-content").html(response.data);
                        }
                    }
                })
                .catch(error => {
                    $("#body-content").html(error.response);
                });

        },
        AddTeamMember: function () {
            $('#loading-panel').removeAttr('hidden');

            var params = new URLSearchParams();
            params.append('team_id', team_menu.team_id);

            axios.get(this.url_add_member, {
                params: params
            }).then(response => {
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
                            SuccessTurnToParentPage(json.Reason, false);
                        }
                    }
                    catch (e) {
                        $("#body-content").html(response.data);
                    }
                }
            }).catch(error => {
                $("#body-content").html(error.response);
            });

            $('#loading-panel').attr('hidden', 'hidden');
        }
    }
});


var vueMembersList = new Vue({
    el: "#members-list",
    data: {
        last_id: 0,
        num: 10,
        url: '../api/GetUsersByTeamId',
        lists: [],
        nextToDoId: 0,
        hasMore: true,
        url_profile: '/UserProfile/Index',
    },
    mounted() {
        this.LoadMore();
    },
    computed: {

    },
    methods: {
        LoadMore: function () {
            if (this.hasMore) {
                $('#members-list').find('#more').html('Loading...<img style="width: 2rem; height: 2rem; margin-left:1rem" src="/sources/img/loading.gif" alt=""/>');

                axios.get(this.url + '/' + team_menu.team_id + '/' + this.num + '/' + this.last_id)
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
                                $.each(json, function (index, item) {
                                    vueMembersList.lists.push({
                                        id: vueTeamsList.nextTodoId++,
                                        uid: item.UId,
                                        img_path: item.Img_Path,
                                        name: item.Name,
                                        age: item.Age,
                                        gender: (item.Gender.toString() == 'Male') ? 'male-icon-large pl-2' : (item.Gender.toString() == 'Female') ? 'female-icon-large pl-2' : '',
                                        position: item.Position,
                                        location: item.Location
                                    });

                                    vueMembersList.last_id = (item.List_Id > vueMembersList.last_id) ? item.List_Id : vueMembersList.last_id;
                                });

                                if (json.length < 10) {
                                    $('#members-list').find('#more').attr('disabled', 'true').removeClass('text-white').addClass('text-muted').html("No more");
                                    vueMembersList.hasMore = false;
                                }
                                else {
                                    $('#members-list').find('#more').removeAttr('disabled').removeClass('text-muted').addClass('text-white').html("More...");
                                    vueMembersList.hasMore = true;
                                }
                            }
                            catch (e) {
                                $("#body-content").html(response.data);
                            }
                        }
                    })
                    .catch(error => {
                        $("#body-content").html(error.response);
                    });
            }
        },
        ToProfile: function (user_id) {
            $('#loading-panel').removeAttr('hidden');

            var params = new URLSearchParams();
            params.append('user_id', user_id);
            params.append('team_id', team_menu.team_id);

            axios.get(this.url_profile, {
                params: params
            }).then(response => {
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
                            alert(json.Reason);
                        }
                    }
                    catch (e) {
                        $("#body-content").html(response.data);
                    }
                }
            }).catch(error => {
                $("#body-content").html(error.response);
            })

            $('#loading-panel').attr('hidden', 'hidden');
        }
    }
});