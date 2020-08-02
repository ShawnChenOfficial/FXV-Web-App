Vue.component('org-list-item', {
    template: '\
            <div class="list-item">\
        <div class="row pointer" v-on:click="$emit(\'to_org_info\')">\
            <div class="col-lg-2 col-md-3 list-item-img-block">\
                <img v-bind:src="img_path" />\
            </div>\
            <div class="col-lg-10 col-md-9 col-12 pb-2">\
                <h3 class="text-FXV">{{name}}</h3>\
                <div class="row text-white">\
                    <p class="col-12">\
                        <span class="material-icons mr-1" style="float:left;line-height: 19px;font-size: 19px;">\
                            location_on\
                        </span> {{location}}\
                    </p>\
                    <p class="col-xl-4 col-lg-4 col-12">\
                        <span class="material-icons mr-1" style="float:left;line-height: 19px;font-size: 19px;">\
                            person\
                        </span> {{manager}}\
                    </p>\
                    <p class="col-xl-4 col-lg-4 col-md-6 col-sm-6 col-12">\
                        <span class="text-muted">\
                            Teams:\
                        </span> {{num_teams}}\
                    </p>\
                    <p class="col-xl-4 col-lg-4 col-md-6 col-sm-6 col-12">\
                        <span class="text-muted">\
                            Members:\
                        </span> {{num_members}}\
                    </p>\
                </div>\
            </div>\
        </div>\
    </div>\
      ',
    props: ['org_id', 'img_path', 'name', 'location', 'manager', 'num_teams', 'num_members'],
});

var vueOrgMenu = new Vue({
    el: "#org-menu",
    data: {
        url_Search: '/api/GetOrgsByName/',
        url_Add: '/Organizations/Create',
        SearchText: '',
    },
    watch: {
        SearchText: 'CheckSearchText',
    },
    methods: {
        CheckSearchText: function () {
            this.SearchText = this.SearchText.toString().replace(/[\\/<>'"]/g, '');
        },
        AddOrg: function () {
            $('#loading-panel').removeAttr('hidden');
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
            $('#loading-panel').attr('hidden', 'hidden');
        },
        Search: _.debounce(
            function () {
                $('#more').html('Loading...<img style="width: 2rem; height: 2rem; margin-left:1rem" src="/sources/img/loading.gif" alt=""/>');
                vueOrgList.lists = [];

                if (this.SearchText == "") {
                    vueOrgList.last_id = 0;
                    vueOrgList.hasMore = true;
                    vueOrgList.LoadMore();
                }
                else {
                    this.SendSearchRequest();
                }
            }, 500),
        SendSearchRequest: function () {
            axios.get(this.url_Search + this.SearchText)
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
                            vueOrgList.lists.push({
                                id: vueOrgList.nextTodoId++,
                                name: item.Name,
                                org_id: item.OrgId,
                                img_path: item.ImgPath,
                                location: item.Location,
                                manager: item.ManagerName,
                                num_teams: item.NumberOfTeams,
                                num_members: item.NumberOfMembers
                            })
                        });
                        $('#more').attr('disabled', 'true').removeClass('text-white').addClass('text-muted').html("No more");
                        vueOrgList.hasMore = false;
                    }
                }).catch(error => {
                    $("#body-content").html(error.response);
                })
        }
    }
});

var vueOrgList = new Vue({
    el: "#list",
    data: {
        last_id: 0,
        num: 10,
        url: '/api/GetOrgList',
        hasMore: true,
        lists: [],
        nextTodoId: 0,
        hasMore: true,
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
                axios.get(this.url + "/" + this.num + "/" + this.last_id)
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
                                vueOrgList.lists.push({
                                    id: vueOrgList.nextTodoId++,
                                    name: item.Name,
                                    org_id: item.OrgId,
                                    img_path: item.ImgPath,
                                    location: item.Location,
                                    manager: item.ManagerName,
                                    num_teams: item.NumberOfTeams,
                                    num_members: item.NumberOfMembers
                                })
                                vueOrgList.last_id = (item.OrgId > vueOrgList.last_id) ? item.OrgId : vueOrgList.last_id;
                            });

                            if (json.length < 10) {
                                $('#more').attr('disabled', 'true').removeClass('text-white').addClass('text-muted').html("No more");
                                vueOrgList.hasMore = false;
                            }
                            else {
                                $('#more').removeAttr('disabled').removeClass('text-muted').addClass('text-white').html("More...");
                                vueOrgList.hasMore = true;
                            }
                        }
                    }).catch(error => {
                        $("#body-content").html(error.response);
                    })
            }
        },
        ToOrgInfo: function (org_id) {
            $('#loading-panel').removeAttr('hidden');

            var params = new URLSearchParams();
            params.append('id', org_id);

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
    },
});