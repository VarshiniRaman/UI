
var app = angular.module('OSSApp', ['ui.grid', 'ui.grid.saveState', 'ui.grid.selection', 'ui.grid.cellNav', 'ui.grid.resizeColumns', 'ui.grid.moveColumns', 'ui.grid.edit']);
app.controller('OSSTable', function ($scope, $http, $sce) {
    //alert("Hi Angular Working !!");
    $scope.test = "myACT";
    $scope.edit_enabled = false;
    $scope.info = false;
    $scope.add_User = false;

    $scope.oss_output = true;
    $scope.backlog = false;
    $scope.sharepoint = false;
    $scope.sap = false;
    $scope.info = false;
    $scope.ShowCopyText = false;

    var Loadingstatus = "";

    //    $scope.failure = false;
    $scope.data = {
        "SAPOrdNo": "",
        "ItemNo": "",
        "Acked Quantity": "",
        "Commit date": "",
        "ReAck Reason Category": "",
        "Free Text Comments": ""
    }
    $scope.userDataCopy = {};
    $scope.userDeleteData = {};
    $scope.addUserdata = {
        "Name": "",
        "Email": "",
        "Active": "",
        "UserLevel": ""
    }
    //alert($scope.test);
    $scope.outputdetails = {
        //data: alldata,
        enableFiltering: true,
        showGridFooter: true,
        //columnDefs: $scope.columnschemaPO
        columnDefs: [
            { name: 'edit', displayName: 'Edit', cellTemplate: '<button id="editBtn" type="button" class="md-fab md-warn md-primary md-raised btn btn-small" style="padding:0px 12px;margin:5px 5px;background-color: #ffffff00;" data-toggle="modal" data-target="#editComments" ng-click="grid.appScope.edit(row.entity)" ><i class="fa fa-pencil-square-o"></i></button>', width: 60, enableCellEdit: false },
            //{ name: 'reset', displayName: 'Reset', cellTemplate: '<button id="resetBtn" type="button" class="md-fab md-warn md-primary md-raised btn btn-small" style="padding:2px 12px;margin:5px 5px;background-color: #0096d6;" data-toggle="modal" data-target="#resetPoints" ng-click="grid.appScope.reset(row.entity)" ><i class="material-icons">autorenew</i></button>', width: 60 },
            //{ name: 'delay', displayName: 'Track', cellTemplate: '<div ng-bind-html="grid.appScope.delayed(row.entity)"> </div>', width: 60 },
            //{ name: 'delayedby', displayName: 'Delayed By', cellTemplate: '<div ng-bind-html="grid.appScope.delayedby(row.entity)"> </div>', width: 100 },
            //{ name: 'edit', displayName: 'Edit', cellTemplate: '<md-icon class="md-secondary" ng-click="grid.appScope.edit(row.entity)" aria-label="Chat"><i class="material-icons">mode_edit</i></md-icon>' },
            { field: 'SAPOrdNo', displayName: 'SAPOrdNo', enableCellEdit: false, width: 120, type: 'number' },
            { field: 'ItemNo', displayName: 'ItemNo', enableCellEdit: false, width: 80, type: 'number' },
            { field: 'AckedQuantity', displayName: 'AckedQuantity', enableCellEdit: false, width: 130 },
            //{ name: 'HP_Points', displayName: 'HP_Tokens', cellTemplate: '<div ng-bind-html="grid.appScope.myHP_Points(row.entity)"> </div>', width: 80 },
            { field: 'Commitdate', displayName: 'Commitdate', enableCellEdit: false, width: 115 },
            { field: 'ReAckReasonCategory', displayName: 'ReAckReasonCategory', enableCellEdit: false, width: 182 },
            { field: 'FreeTextComments', name: 'Delayed', displayName: 'Delayed', cellTemplate: '<div ng-bind-html="grid.appScope.delayDisplay(row.entity)" style="text-align: center;"></div>', width: 85, enableCellEdit: false },
            { field: 'FreeTextComments', displayName: 'WeekNumber', cellTemplate: '<div style="text-align: center;">{{grid.appScope.getDelayedCellValue(row.entity)}}</div>', sortingAlgorithm: function (a, b) {

                    var stra = String(a).toLowerCase();
                    var strb = String(b).toLowerCase();

                    var weekNumbera = 0;
                    var weekNumberb = 0;

                    var NUMBER_GROUPS = /(-?\d*\.?\d+)/g;

                    if (stra.includes("week-")) {
                        weekNumbera = stra.substring(stra.lastIndexOf("week-") + 5);
                        weekNumbera = Number(weekNumbera);
                    }
                    else if (stra.includes("week -")) {
                        weekNumbera = stra.substring(stra.lastIndexOf("week -") + 6);
                        weekNumbera = Number(weekNumbera);
                    }
                    else if (stra.includes("wk")) {
                        weekNumbera = stra.substring(stra.lastIndexOf("wk") + 2);
                        weekNumbera = Number(weekNumbera);
                    }
                    else if (stra.includes("week")) {
                        weekNumbera = stra.substring(stra.lastIndexOf("week") + 4);
                        weekNumbera = Number(weekNumbera);
                    }

                    if (strb.includes("week-")) {
                        weekNumberb = strb.substring(strb.lastIndexOf("week-") + 5);
                        weekNumberb = Number(weekNumberb);
                    }
                    else if (strb.includes("week -")) {
                        weekNumberb = strb.substring(strb.lastIndexOf("week -") + 6);
                        weekNumberb = Number(weekNumberb);
                    }
                    else if (strb.includes("wk")) {
                        weekNumberb = strb.substring(strb.lastIndexOf("wk") + 2);
                        weekNumberb = Number(weekNumberb);
                    }
                    else if (strb.includes("week")) {
                        weekNumberb = strb.substring(strb.lastIndexOf("week") + 4);
                        weekNumberb = Number(weekNumberb);
                    }

                    var aa = String(weekNumbera).split(NUMBER_GROUPS),
                        bb = String(weekNumberb).split(NUMBER_GROUPS),
                        min = Math.min(aa.length, bb.length);

                        for (var i = 0; i < min; i++) {
                            var x = parseFloat(aa[i]) || aa[i].toLowerCase(),
                            y = parseFloat(bb[i]) || bb[i].toLowerCase();
                            if (x < y) return -1;
                            else if (x > y) return 1;
                        }

                        return 0;
                }, enableCellEdit: false, width: 120
            },
            { field: 'FreeTextComments', displayName: 'FreeTextComments', enableCellEdit: true, width: 334, enableCellEditOnFocus :true},
        ]
    };
  
    $scope.backlogdetails = {
        //data: alldata,
        enableFiltering: true,
        showGridFooter: true,
        columnDefs: $scope.columnschemaBacklog
    };

    $scope.sharepointdetails = {
        //data: alldata,
        enableFiltering: true,
        showGridFooter: true,
        columnDefs: $scope.columnschemaSharePoint
    };

    $scope.sapdetails = {
        //data: alldata,
        enableFiltering: true,
        showGridFooter: true,
        columnDefs: $scope.columnschemaSAP
    };

    $scope.userdetails = {
        //data: alldata,
        enableFiltering: true,
        showGridFooter: true,
        columnDefs: [
            { field: 'Edit', displayName: 'Edit', cellTemplate: '<button id="editBtn" type="button" class="md-fab md-warn md-primary md-raised btn btn-small" style="padding:0px 12px;margin:5px 5px;background-color: #ffffff00;" data-toggle="modal" data-target="#editUser" ng-click="grid.appScope.editUser(row.entity)" ><i class="fa fa-pencil-square-o"></i></button>', width: 65 },
            { field: 'Delete', displayName: 'Delete', cellTemplate: '<button id="deleteBtn" type="button" class="md-fab md-warn md-primary md-raised btn btn-small" style="padding:0px 12px;margin:5px 5px;background-color: #ffffff00;" data-toggle="modal" data-target="#deleteUser" ng-click="grid.appScope.deleteUser(row.entity)" ><i class="fa fa-trash-o"></i></button>', width: 82 },
            { field: 'ID', displayName: 'ID', enableCellEdit: false, type: 'number' },//, width: 80
            { field: 'UserLevel', displayName: 'UserLevel', enableCellEdit: false },//, width: 105
            { field: 'Name', displayName: 'Name', enableCellEdit: false },//, width: 175
            { field: 'Email', displayName: 'Email', enableCellEdit: false },//, width: 175
            { field: 'Active', displayName: 'Active', enableCellEdit: false },//, width: 80
            { field: 'CreatedBy', displayName: 'CreatedBy', enableCellEdit: false },//, width: 130
            { field: 'Createddate', displayName: 'CreatedDate', enableCellEdit: false },//, width: 102
            { field: 'ModifiedBy', displayName: 'ModifiedBy', enableCellEdit: false },//, width: 130
            { field: 'Modifieddate', displayName: 'ModifiedDate', enableCellEdit: false },//, width: 102
        ]
    };

    $scope.copied = {
        showSelectionCheckbox: true,
        enableRowSelection: true,
        //selectWithCheckboxOnly: false,
        enableFiltering: true,
        showGridFooter: true,
        onRegisterApi: function(gridApi) { //register grid data first within the gridOptions
            $scope.gridApi = gridApi;
        },
        columnDefs: $scope.columnschemaCopied
    };

    $scope.pointsaccumulated = {
        //data: alldata,
        enableFiltering: true,
        //columnDefs: $scope.columnschemaPO
        columnDefs: [
            //{ name: 'edit', displayName: 'Edit', cellTemplate: '<button id="editBtn" type="button" class="md-fab md-warn md-primary md-raised btn btn-small" style="padding:2px 12px;margin:5px 5px;background-color: #0096d6;" data-toggle="modal" data-target="#editAccess" ng-click="grid.appScope.edit(row.entity)" ><i class="material-icons">mode_edit</i></button>', width: 60 },
            { name: 'reset', displayName: 'Reset', cellTemplate: '<button id="resetBtn" type="button" class="md-fab md-warn md-primary md-raised btn btn-small" style="padding:2px 12px;margin:5px 5px;background-color: #0096d6;" data-toggle="modal" data-target="#resetPoints" ng-click="grid.appScope.reset(row.entity)" ><i class="material-icons">autorenew</i></button>', width: 60 },
            //{ name: 'delay', displayName: 'Track', cellTemplate: '<div ng-bind-html="grid.appScope.delayed(row.entity)"> </div>', width: 60 },
            //{ name: 'delayedby', displayName: 'Delayed By', cellTemplate: '<div ng-bind-html="grid.appScope.delayedby(row.entity)"> </div>', width: 100 },
            //{ name: 'edit', displayName: 'Edit', cellTemplate: '<md-icon class="md-secondary" ng-click="grid.appScope.edit(row.entity)" aria-label="Chat"><i class="material-icons">mode_edit</i></md-icon>' },
            { field: 'Created_By', displayName: 'Created_By' },
            { name: 'Accumulated_MyACT_Points', displayName: 'Accumulated_MyACT_Points', cellTemplate: '<div ng-bind-html="grid.appScope.myAct_Accumulated_UnitsDisplay(row.entity)" style="text-align: center;"> </div>', width: 80 },
            { name: 'HP_Points', displayName: 'HP_Tokens', cellTemplate: '<div ng-bind-html="grid.appScope.myHP_Points_Total(row.entity)"> </div>', width: 80 },
            { field: 'Accumulated_MyACT_Points', displayName: 'Accumulated_MyACT_Points' },
            { field: 'Reporting_Manager', displayName: 'Reporting_Manager' },
            { field: 'L4Manager', displayName: 'L4Manager' }
        ]
    };

    $scope.tracker_details = {
        //data: alldata,
        enableFiltering: true,
        //columnDefs: $scope.columnschemaPO
        columnDefs: [
            //{ name: 'reset', displayName: 'Reset', cellTemplate: '<button id="resetBtn" type="button" class="md-fab md-warn md-primary md-raised btn btn-small" style="padding:2px 12px;margin:5px 5px;background-color: #0096d6;" data-toggle="modal" data-target="#resetPoints" ng-click="grid.appScope.reset(row.entity)" ><i class="material-icons">autorenew</i></button>', width: 60 },
            { field: 'Tracker_ID', displayName: 'Created_By' },
            { field: 'ModifiedBy', displayName: 'ModifiedBy' },
            { field: 'Modified_Date', displayName: 'Modified_Date' },
            { field: 'OldValue', displayName: 'OldValue' },
            { field: 'NewValue', displayName: 'NewValue' }
        ]
    };

    $scope.getUser = function () {
        $http({
            method: "POST",
            url: "/getCurrentUser.asmx/getUserLDAP",
            data: JSON.stringify({ data: '' })
        }).then(function (results) {
           // console.log(JSON.stringify(results));
           // console.log(results.data.d.NtID);
            $scope.userNtid = results.data.d.NtID;
            $scope.userName = results.data.d.FullName;
            $scope.userMailId = results.data.d.WorkEmail;
            return results;
        }).catch(function (response) {
            if (response.status === 408) {
                // alert("408: Error Loading Catalog" + JSON.stringify(response), "Error", "fail");

                $scope.info = true;
                $.notify({
                    icon: "notifications",
                    message: JSON.stringify(response)

                });

            }
            else {
                // alert("Error Loading Catalog" + JSON.stringify(response), "Error", "fail");

                $scope.info = true;
                $.notify({
                    icon: "notifications",
                    message: JSON.stringify(response)

                });

            }

        });
    };

    $scope.getUser();

    $scope.loadOutput = function (rec) {
        Loadingstatus = "Loading the OSS Output"
        start();
        postdata = JSON.stringify(
            {
                //currentuser: $scope.userNtid,
                tablename: "OSS_Output_View",
                task: "retrieveData",
                condition: "",
                setfields: "NA"
                //  strictmyview: strictmyview
            });
        $http({
            method: "POST",
            url: "/webservices/Manage.asmx/retrieveBacklog",

            data: JSON.stringify({ data: postdata })
        }).then(function (results) {
          
            //console.log("Hi"+JSON.stringify(results));
            if (results.data.d.rowData != null) {

                $scope.outputdetails.data = results.data.d.rowData;
               // alert("output " + JSON.stringify($scope.outputdetails.data));
                //stop();
                if ($scope.outputdetails.data == null || $scope.outputdetails.data == '' || $scope.outputdetails.data == undefined)
                    $scope.commentstotal = 0;
                else
                    $scope.commentstotal = $scope.outputdetails.data.length;

            }




                stop();
            return results;
        }).catch(function (response) {
            if (response.status === 408) {
                //alert("Error 408 :"+JSON.stringify(response));

                $scope.info = true;
                $.notify({
                    icon: "notifications",
                    message: "Error 408 :" + JSON.stringify(response)

                });
                // Model.errorMessage("Error 408 : Error loading POs" + JSON.stringify(response), "Error", "fail");
                stop();
            }
            else {
                //alert(JSON.stringify(response));
                $scope.info = true;
                $.notify({
                    icon: "notifications",
                    message: JSON.stringify(response)

                });
                //Model.errorMessage("Error loading POs" + JSON.stringify(response), "Error", "fail");
                stop();
            }

        });;
    }

    $scope.loadOutput();

    $scope.loadBacklog = function (rec) {
        Loadingstatus = "Loading the Backlog Input"
        start();
        postdata = JSON.stringify(
            {
                //currentuser: $scope.userNtid,
                tablename: "Backlog",
                task: "Retreive_Backlog_Data",
                condition: "",
                setfields: "NA"
                //  strictmyview: strictmyview
            });
        $http({
            method: "POST",
            url: "/webservices/Manage.asmx/retrieveBacklog",

            data: JSON.stringify({ data: postdata })
        }).then(function (results) {
            $scope.columnschemaBacklog = [];
            if (results.data.d.rowData != null) {

                //stop();
              //  alert("Backlog" + JSON.stringify(results.data.d.rowData));
                $scope.backlogdetails.data = results.data.d.rowData;
                if ($scope.backlogdetails.data == null || $scope.backlogdetails.data == '' || $scope.backlogdetails.data == undefined)
                    $scope.backlogtotal = 0;
                else
                    $scope.backlogtotal = $scope.backlogdetails.data.length;

               // console.log($scope.backlogdetails.data);
                angular.forEach(results.data.d.rowData[0], function (value, key) {

                    if (key.indexOf('$') != 0)
                        $scope.columnschemaBacklog.push({ field: key, displayName: key});
                    else
                        $scope.columnschemaBacklog.push({ field: key, displayName: key, visible: false });

                });
            }
                stop();
            return results;
        }).catch(function (response) {
            if (response.status === 408) {
                //alert("Error 408 :"+JSON.stringify(response));

                $scope.info = true;
                $.notify({
                    icon: "notifications",
                    message: "Error 408 :" + JSON.stringify(response)

                });
                // Model.errorMessage("Error 408 : Error loading POs" + JSON.stringify(response), "Error", "fail");
                stop();
            }
            else {
                //alert(JSON.stringify(response));
                $scope.info = true;
                $.notify({
                    icon: "notifications",
                    message: JSON.stringify(response)

                });
                //Model.errorMessage("Error loading POs" + JSON.stringify(response), "Error", "fail");
                stop();
            }

        });;
    }

    $scope.loadBacklog();

    $scope.loadSharePoint = function (rec) {
        Loadingstatus = "Loading the SharePoint Input"
        start();
        postdata = JSON.stringify(
            {
                //currentuser: $scope.userNtid,
                tablename: "SharePoint_comments_Table",
                task: "retreive_SharePoint_Data",
                condition: "",
                setfields: "NA"
                //  strictmyview: strictmyview
            });
        $http({
            method: "POST",
            url: "/webservices/Manage.asmx/retrieveBacklog",

            data: JSON.stringify({ data: postdata })
        }).then(function (results) {
            $scope.columnschemaSharePoint = [];
            if (results.data.d.rowData != null) {

                //stop();
                //alert("userdetails" + JSON.stringify($scope.userdetails.data));
                $scope.sharepointdetails.data = results.data.d.rowData;
                if ($scope.sharepointdetails.data == null || $scope.sharepointdetails.data == '' || $scope.sharepointdetails.data == undefined)
                    $scope.sharepointtotal = 0;
                else
                    $scope.sharepointtotal = $scope.sharepointdetails.data.length;

                angular.forEach(results.data.d.rowData[0], function (value, key) {

                    if (key.indexOf('$') != 0)
                        $scope.columnschemaSharePoint.push({ field: key, displayName: key });
                    else
                        $scope.columnschemaSharePoint.push({ field: key, displayName: key, visible: false });

                });
            }
                stop();
            return results;
        }).catch(function (response) {
            if (response.status === 408) {
                //alert("Error 408 :"+JSON.stringify(response));

                $scope.info = true;
                $.notify({
                    icon: "notifications",
                    message: "Error 408 :" + JSON.stringify(response)

                });
                // Model.errorMessage("Error 408 : Error loading POs" + JSON.stringify(response), "Error", "fail");
                stop();
            }
            else {
                //alert(JSON.stringify(response));
                $scope.info = true;
                $.notify({
                    icon: "notifications",
                    message: JSON.stringify(response)

                });
                //Model.errorMessage("Error loading POs" + JSON.stringify(response), "Error", "fail");
                stop();
            }

        });;
    }

    $scope.loadSharePoint();

    $scope.loadSAP = function (rec) {
        Loadingstatus = "Loading the SAP Input"
        start();
        postdata = JSON.stringify(
            {
                //currentuser: $scope.userNtid,
                tablename: "SAP_Comments_Table",
                task: "retreive_SAP_Comments_Data",
                condition: "",
                setfields: "NA"
                //  strictmyview: strictmyview
            });
        $http({
            method: "POST",
            url: "/webservices/Manage.asmx/retrieveBacklog",

            data: JSON.stringify({ data: postdata })
        }).then(function (results) {
            $scope.columnschemaSAP = [];
            if (results.data.d.rowData != null) {

                $scope.sapdetails.data = results.data.d.rowData;
                if ($scope.sapdetails.data == null || $scope.sapdetails.data == '' || $scope.sapdetails.data == undefined)
                    $scope.saptotal = 0;
                else
                    $scope.saptotal = $scope.sapdetails.data.length;

                angular.forEach(results.data.d.rowData[0], function (value, key) {

                    if (key.indexOf('$') != 0)
                        $scope.columnschemaSAP.push({ field: key, displayName: key });
                    else
                        $scope.columnschemaSAP.push({ field: key, displayName: key, visible: false });

                });
                stop();
            }
            return results;
        }).catch(function (response) {
            if (response.status === 408) {
                //alert("Error 408 :"+JSON.stringify(response));

                $scope.info = true;
                $.notify({
                    icon: "notifications",
                    message: "Error 408 :" + JSON.stringify(response)

                });
                // Model.errorMessage("Error 408 : Error loading POs" + JSON.stringify(response), "Error", "fail");
                stop();
            }
            else {
                //alert(JSON.stringify(response));
                $scope.info = true;
                $.notify({
                    icon: "notifications",
                    message: JSON.stringify(response)

                });
                //Model.errorMessage("Error loading POs" + JSON.stringify(response), "Error", "fail");
                stop();
            }

        });;
    }

    $scope.loadSAP();

    $scope.loadUsers = function (rec) {
        Loadingstatus = "Loading the User List"
        start();
        postdata = JSON.stringify(
            {
                //currentuser: $scope.userNtid,
                tablename: "UserDetails",
                task: "userDetails",
                condition: "",
                setfields: "NA"
                //  strictmyview: strictmyview
            });
        $http({
            method: "POST",
            url: "/webservices/Manage.asmx/retrieveBacklog",

            data: JSON.stringify({ data: postdata })
        }).then(function (results) {
            //$scope.columnschemaUsers = [];
            if (results.data.d.rowData != null) {

                $scope.userdetails.data = results.data.d.rowData;
                //$scope.userdetails.columnDefs.push({ field: 'Edit', displayName: 'Edit', cellTemplate: '<button id="editBtn" type="button" class="md-fab md-warn md-primary md-raised btn btn-small" style="padding:0px 12px;margin:5px 5px;background-color: #ffffff00;" data-toggle="modal" data-target="#editUser" ng-click="grid.appScope.editUser(row.entity)" ><i class="fa fa-pencil-square-o"></i></button>', width: 60 });
                //angular.forEach(results.data.d.rowData[0], function (value, key) {
                //    if (key.indexOf('$') != 0)
                //        $scope.userdetails.columnDefs.push({ field: key, displayName: key });
                //    else
                //        $scope.userdetails.columnDefs.push({ field: key, displayName: key, visible: false });

                //});
                stop();
            }
            return results;
        }).catch(function (response) {
            if (response.status === 408) {
                //alert("Error 408 :"+JSON.stringify(response));

                $scope.info = true;
                $.notify({
                    icon: "notifications",
                    message: "Error 408 :" + JSON.stringify(response)

                });
                // Model.errorMessage("Error 408 : Error loading POs" + JSON.stringify(response), "Error", "fail");
                stop();
            }
            else {
                //alert(JSON.stringify(response));
                $scope.info = true;
                $.notify({
                    icon: "notifications",
                    message: JSON.stringify(response)

                });
                //Model.errorMessage("Error loading POs" + JSON.stringify(response), "Error", "fail");
                stop();
            }

        });;
    }

    $scope.loadUsers();

    //$scope.$watch('userdetails.columnDefs', function (colDef, old) {
    //    console.log("coldef change");
    //    console.log(colDef);
    //    console.log(old);
    //    $scope.$$childHead.grid.refresh();
    //});

    $scope.show_oss = function () {
        $scope.oss_output = true;
        $scope.backlog = false;
        $scope.sharepoint = false;
        $scope.sap = false;
    }
    $scope.show_backlog = function () {
        $scope.oss_output = false;
        $scope.backlog = true;
        $scope.sharepoint = false;
        $scope.sap = false;
    }
    $scope.show_sharepoint = function () {
        $scope.oss_output = false;
        $scope.backlog = false;
        $scope.sharepoint = true;
        $scope.sap = false;
    }
    $scope.show_sap = function () {
        $scope.oss_output = false;
        $scope.backlog = false;
        $scope.sharepoint = false;
        $scope.sap = true;
    }
    $scope.loadTracker = function (rec) {
        Loadingstatus = ""
        start();
        postdata = JSON.stringify(
            {
                //currentuser: $scope.userNtid,
                tablename: "myACT_DATA",
                task: "retreive_tracker",
                condition: "",
                setfields: "NA"
                //  strictmyview: strictmyview
            });
        $http({
            method: "POST",
            url: "/webservices/Manage.asmx/retrieveBacklog",

            data: JSON.stringify({ data: postdata })
        }).then(function (results) {

            if (results.data.d.rowData != null) {

                $scope.tracker_details.data = results.data.d.rowData;
                stop();
            }
            return results;
        }).catch(function (response) {
            if (response.status === 408) {
                //alert("Error 408 :"+JSON.stringify(response));

                $scope.info = true;
                $.notify({
                    icon: "notifications",
                    message: "Error 408 :" + JSON.stringify(response)

                });
                // Model.errorMessage("Error 408 : Error loading POs" + JSON.stringify(response), "Error", "fail");
                stop();
            }
            else {
                //alert(JSON.stringify(response));
                $scope.info = true;
                $.notify({
                    icon: "notifications",
                    message: JSON.stringify(response)

                });
                //Model.errorMessage("Error loading POs" + JSON.stringify(response), "Error", "fail");
                stop();
            }

        });;
    }

  //  $scope.loadTracker();

    $scope.reset = function (row, type) {

        // alert("edit " + JSON.stringify(row));

        $scope.reset_enabled = true;
        $scope.reset_data = JSON.stringify(row["Created_By"]);

        for (var i in row)
            $scope.data[i] = row[i];

        $scope.resetdatacopy = angular.copy($scope.data);
        //  alert($scope.resetdatacopy);

    }
    $scope.resetSubmit = function (userData) {
        //  alert(JSON.stringify(userData));
        var cond = "where Created_By='" + userData["Created_By"] + "' and Reporting_Manager='" + userData["Reporting_Manager"] + "' and L4Manager='" + userData["L4Manager"] + "'";
        var Accumulated_MYACT_Points = userData["Accumulated_MyACT_Points"]

        postdata = JSON.stringify(
            {
                currentuser: $scope.userMailId,
                tablename: "UserDetails",
                task: "resetPoints",
                condition: cond,
                setfields: Accumulated_MYACT_Points,
                user: $scope.userName,

                //val: JSON.stringify(userData)
                //  strictmyview: strictmyview
            });
        $http({
            method: "POST",
            url: "/webservices/Manage.asmx/deleteUser",

            data: JSON.stringify({ data: postdata })
        }).then(function (results) {
            // alert(JSON.stringify(results));
            if (results.data.d != null) {

                $scope.result = results.data.d;
                var output = JSON.stringify(results.data.d);
                //alert("Result " + JSON.stringify($scope.result));
                //$scope.MessageSuccess = $scope.result;
                $scope.info = true;
                color = Math.floor((Math.random() * 4) + 1);

                $.notify({
                    icon: "notifications",
                    message: "Points reset to 0 for " + userData["Created_By"]

                });
                $scope.loadPoints();
                $scope.reset_enabled = false;
            }

            return results;
        }).catch(function (response) {
            if (response.status === 408) {
                // alert("Error 408 :" + JSON.stringify(response));
                //$scope.MessageFailure = JSON.stringify(response);
                $scope.info = true;
                $.notify({
                    icon: "notifications",
                    message: JSON.stringify(response)

                });
            }
            else {
                // alert(JSON.stringify(response));
                //$scope.MessageFailure = JSON.stringify(response);
                $scope.info = true;
                $.notify({
                    icon: "notifications",
                    message: JSON.stringify(response)

                });
            }

        });;
    }

    $scope.edit = function (row, type) {

        //alert("edit " + JSON.stringify(row));

        $scope.edit_enabled = true;
        $scope.edit_data = JSON.stringify(row["ID"]);

        for (var i in row)
            $scope.data[i] = row[i];

        $scope.datacopy = angular.copy($scope.data);
        //alert(JSON.stringify($scope.datacopy));

        //$mdDialog.show({
        //    contentElement: '#edit_req',
        //    parent: angular.element(document.body),
        //    targetEvent: row,
        //    clickOutsideToClose: true
        //});
    }

    $scope.editSubmit = function (userData) {
        //alert(JSON.stringify(userData));
        Loadingstatus = "Updating Comments"
        start();
        var condition = "set OSS_Comments='" + userData["FreeTextComments"] + "', ModifiedBy='" + $scope.userNtid + "', ModifiedDateTime=GETDATE() where SalesOrderId='" + userData["SAPOrdNo"] + "' and SalesOrderItemId='" + userData["ItemNo"] + "' and GoodsIssueQuantityEA='" + userData["AckedQuantity"] + "'";
        //alert(condition);
        postdata = JSON.stringify(
            {
                currentuser: $scope.userMailId,
                tablename: "UserDetails",
                task: "updateComments",
                condition: condition,
                setfields: "NA",
                user: $scope.userName,
                //val: JSON.stringify(userData)
                //  strictmyview: strictmyview
            });
        $http({
            method: "POST",
            url: "/webservices/Manage.asmx/updateBacklogComments",

            data: JSON.stringify({ data: postdata })
        }).then(function (results) {
            //alert(JSON.stringify(results));
            if (results.data.d != null) {
                $('#editComments').modal('toggle');
                $scope.result = results.data.d;
                stop();
                //alert("result" + JSON.stringify($scope.result));
                //$scope.MessageSuccess = $scope.result;
                $scope.info = true;
                color = Math.floor((Math.random() * 4) + 1);

                $.notify({
                    icon: "notifications",
                    message: results.data.d + " for " + userData["SAPOrdNo"]

                });
                //$scope.info = true;
                //color = Math.floor((Math.random() * 4) + 1);

                //$.notify({
                //    icon: "notifications",
                //    message: $scope.result

                //}, {
                //    type: type[color],
                //    timer: 4000,
                //    placement: {
                //        from: from,
                //        align: align
                //    }
                //});

            }
            return results;
        }).catch(function (response) {
            stop();
            if (response.status === 408) {
                //alert("Error 408 :" + JSON.stringify(response));
                //$scope.MessageFailure = JSON.stringify(response);
                $scope.info = true;
                $.notify({
                    icon: "notifications",
                    message: response

                });
            }
            else {
                // alert(JSON.stringify(response));
                //$scope.MessageFailure = JSON.stringify(response);
                $scope.info = true;
                $.notify({
                    icon: "notifications",
                    message: response

                });
            }

        });;
    }

    $scope.editUser = function (row, type) {

        for (var i in row)
            $scope.userDataCopy[i] = row[i];

        $scope.usercopy = angular.copy($scope.userDataCopy);
        //alert(JSON.stringify($scope.datacopy));
    }

    $scope.editUserSubmit = function (userData) {
        //alert(JSON.stringify(userData));
        Loadingstatus = "Updating Access"
        start();
        var condition = "set UserLevel='" + userData["UserLevel"] + "',Active='" + userData["Active"] + "', ModifiedBy='" + $scope.userNtid + "', ModifiedDate=GETDATE() where Email='" + userData["Email"] + "'";
        //alert(condition);
        postdata = JSON.stringify(
            {
                currentuser: $scope.userMailId,
                tablename: "UserDetails",
                task: "updateUser",
                condition: condition,
                setfields: "NA",
                user: $scope.userName,
                //val: JSON.stringify(userData)
                //  strictmyview: strictmyview
            });
        $http({
            method: "POST",
            url: "/webservices/Manage.asmx/updateBacklogComments",

            data: JSON.stringify({ data: postdata })
        }).then(function (results) {
            //alert(JSON.stringify(results));
            if (results.data.d != null) {
                $('#editUser').modal('toggle');
                stop();
                //alert("result" + JSON.stringify($scope.result));
                //$scope.MessageSuccess = $scope.result;
                $scope.info = true;
                color = Math.floor((Math.random() * 4) + 1);

                $.notify({
                    icon: "notifications",
                    message: results.data.d + " for " + userData["Name"]

                });
            }
            $scope.loadUsers();
            return results;
        }).catch(function (response) {
            stop();
            if (response.status === 408) {
                //alert("Error 408 :" + JSON.stringify(response));
                //$scope.MessageFailure = JSON.stringify(response);
                $scope.info = true;
                $.notify({
                    icon: "notifications",
                    message: JSON.stringify(response)

                });
            }
            else {
                // alert(JSON.stringify(response));
                //$scope.MessageFailure = JSON.stringify(response);
                $scope.info = true;
                $.notify({
                    icon: "notifications",
                    message: JSON.stringify(response)

                });
            }

        });;
    }

    $scope.addUserSubmit = function (userData) {
        //alert(JSON.stringify(data));
        var condition = "('" + userData["UserLevel"] + "','" + userData["Name"] + "','" + userData["Email"] + "','" + userData["Active"] + "',GETDATE(),GETDATE(),'" + $scope.userNtid + "','" + $scope.userNtid + "')";
     //   alert(condition);
        postdata = JSON.stringify(
            {
                currentuser: $scope.userNtid,
                tablename: "UserDetails",
                task: "insertUser",
                condition: condition,
                setfields: "NA",
                //user: $scope.userName,
                val: JSON.stringify(userData)
                //  strictmyview: strictmyview
            });
        $http({
            method: "POST",
            url: "/webservices/Manage.asmx/updateBacklogComments",

            data: JSON.stringify({ data: postdata })
        }).then(function (results) {
           // alert(JSON.stringify(results));
            if (results.data.d != null) {
                $('#addUser').modal('toggle');
                stop();
                $scope.info = true;
                color = Math.floor((Math.random() * 4) + 1);

                $.notify({
                    icon: "notifications",
                    message: results.data.d + ": Added Access for " + userData["Name"]

                });
            }
            $scope.loadUsers();
            return results;
        }).catch(function (response) {
           // alert(JSON.stringify(response));
            if (response.status === 408) {
                console.log("Error 408 :" + JSON.stringify(response));
                //$scope.MessageFailure = JSON.stringify(response);
                $scope.info = true;
                $.notify({
                    icon: "notifications",
                    message: response

                });
            }
            else {
                console.log(JSON.stringify(response));
                //$scope.MessageFailure = JSON.stringify(response);
                $scope.info = true;
                $.notify({
                    icon: "notifications",
                    message: response

                });
            }

        });;
    }

    $scope.deleteUser = function (row, type) {

        for (var i in row)
            $scope.userDeleteData[i] = row[i];

        $scope.deletedatacopy = angular.copy($scope.userDeleteData);

    }
    $scope.deleteUserSubmit = function (userData) {
     //   alert(JSON.stringify(userData));
        // alert($scope.deleteOption);
        var cond = " where Email='"+userData["Email"]+"'";
        //if ($scope.deleteOption == "access") {
        //    cond = "where Sno=" + userData["Sno"];
        //}
        //else if ($scope.deleteOption == "user") {
        //    cond = "where Name='" + userData["Name"] + "'and Email='" + userData["Email"] + "'";
        //}
        postdata = JSON.stringify(
            {
                currentuser: $scope.userNtid,
                tablename: "UserDetails",
                task: "deleteUser",
                condition: cond,
                setfields: "NA",
                //user: $scope.userName,
                //val: JSON.stringify(userData)
                //  strictmyview: strictmyview
            });
        $http({
            method: "POST",
            url: "/webservices/Manage.asmx/deleteUser",

            data: JSON.stringify({ data: postdata })
        }).then(function (results) {
         //   alert(JSON.stringify(results));
            if (results.data.d != null) {
                $('#deleteUser').modal('toggle');
                stop();
                $scope.info = true;
                color = Math.floor((Math.random() * 4) + 1);

                $.notify({
                    icon: "notifications",
                    message: results.data.d + ": Deleted Access for " + userData["Name"]

                });
            }
            $scope.loadUsers();
            return results;
        }).catch(function (response) {
            if (response.status === 408) {
                // alert("Error 408 :" + JSON.stringify(response));
                //$scope.MessageFailure = JSON.stringify(response);
                $scope.info = true;
                $.notify({
                    icon: "notifications",
                    message: JSON.stringify(response)

                });
            }
            else {
                // alert(JSON.stringify(response));
                //$scope.MessageFailure = JSON.stringify(response);
                $scope.info = true;
                $.notify({
                    icon: "notifications",
                    message: JSON.stringify(response)

                });
            }

        });;
    }
    $scope.closepopup = function () {
        $scope.edit_enabled = false;
        $scope.delete_enabled = false;
        $scope.topperView_enabled = false;
        $scope.reset_enabled = false;
    }

    $scope.fixGridInTab = function () {
        window.setTimeout(function () {
            $(window).resize();
            $(window).resize();
        }, 1000);
    };

    $scope.close = function () {
        $("#showPO_popup").hide();
    }
    $scope.exportOutputData = function () {
        //console.log($scope.outputdetails.data);
        var filename = "OSS_Output.xlsx";
        alasql('SELECT * INTO XLSX("' + filename + '",{headers:true}) FROM ?', [$scope.outputdetails.data]);
    };
    $scope.exportBacklogData = function () {
        var filename = "BacklogInput.xlsx";
        alasql('SELECT * INTO XLSX("' + filename + '",{headers:true}) FROM ?', [$scope.backlogdetails.data]);
    }
    $scope.exportSharePointData = function () {
        var filename = "SharePointInput.xlsx";
        alasql('SELECT * INTO XLSX("' + filename + '",{headers:true}) FROM ?', [$scope.sharepointdetails.data]);
    }
    $scope.exportSAPData = function () {
        alasql('SELECT * INTO XLSX("SAPInput.xlsx",{headers:true}) FROM ?', [$scope.sapdetails.data]);
    };
    
    $scope.exportUserData = function () {
        alasql('SELECT * INTO XLSX("TOBBIEUsers.xlsx",{headers:true}) FROM ?', [$scope.userdetails.data]);
    }
    $scope.getWeekNumber = function () {
        var n = new Date();
        // Copy date so don't modify original
        var d = new Date(Date.UTC(n.getFullYear(), n.getMonth(), n.getDate()));
        // Set to nearest Thursday: current date + 4 - current day number
        // Make Sunday's day number 7
        d.setUTCDate(d.getUTCDate() + 4 - (d.getUTCDay() || 7));
        // Get first day of year
        var yearStart = new Date(Date.UTC(d.getUTCFullYear(), 0, 1));
        // Calculate full weeks to nearest Thursday
        var weekNo = Math.ceil((((d - yearStart) / 86400000) + 1) / 7);
        
        $scope.currentWeekNo = weekNo;
        //alert($scope.currentWeekNo);
    }
    $scope.getWeekNumber();
    $scope.delayDisplay = function (rec) {
        var getHtml = ""
        console.log(JSON.stringify(rec));
        //console.log(row["myACT_Units"]);
        //console.log(rec.myACT_Units);
        var comments = rec["FreeTextComments"];
        var weekNum = 0;
        var str = comments.toLowerCase();
        if (str.includes("week-")) {
            weekNum = str.substring(str.lastIndexOf("week-") + 5);
            weekNum = Number(weekNum);
        }
        else if (str.includes("week -")) {
            weekNum = str.substring(str.lastIndexOf("week -") + 6);
            weekNum = Number(weekNum);
        }
        else if (str.includes("wk")) {
            weekNum = str.substring(str.lastIndexOf("wk") + 2);
            weekNum = Number(weekNum);
        }
        else if (str.includes("week")) {
            weekNum = str.substring(str.lastIndexOf("week") + 4);
            weekNum = Number(weekNum);
        }
        if (Number(weekNum) > $scope.currentWeekNo) {
            getHtml += "<i class='fa fa-battery-full' style='margin-top: 3px;font-size: 25px;color: green'></i>"
        }
        if (Number(weekNum) == $scope.currentWeekNo) {
            getHtml += "<i class='fa fa-battery-half' style='margin-top: 3px;font-size: 25px;color: orange'></i>"
        }
        if (Number(weekNum) < $scope.currentWeekNo && Number(weekNum) > 0) {
            getHtml += "<i class='fa fa-battery-empty' style='margin-top: 3px;font-size: 25px;color: red'></i>"
        }
        if (Number(weekNum) == 0) {
            getHtml += "<i class='fa fa-exclamation-circle' style='margin-top: 3px;font-size: 25px;color: #ffd600'></i>"
        }
        //if (rec.actual_finish_date > rec.completion_date)
        //    gethtml += "<i class='material-icons text-danger'style='margin-top: 3px;margin-left: 15px;font-size: 27px;'>warning</i>"

        //if (rec.actual_finish_date < rec.completion_date || rec.actual_finish_date == rec.completion_date)
        //    gethtml += "<i style='margin-top: 3px;margin-left: 18px;color: green;font-size: 20px;' class='glyphicon glyphicon-ok'></i>"

        return $sce.trustAsHtml(getHtml);
    }

    $scope.getDelayedCellValue = function (rec) {
        //console.log(JSON.stringify(rec));
        var cellValue = 0;
        var comments = rec["FreeTextComments"];
        var weekNumber = 0;
        var str = comments.toLowerCase();
        if (str.includes("week-")) {
            weekNumber = str.substring(str.lastIndexOf("week-") + 5);
            weekNumber = Number(weekNumber);
        }
        else if (str.includes("week -")) {
            weekNumber = str.substring(str.lastIndexOf("week -") + 6);
            weekNumber = Number(weekNumber);
        }
        else if (str.includes("wk")) {
            weekNumber = str.substring(str.lastIndexOf("wk") + 2);
            weekNumber = Number(weekNumber);
        }
        else if (str.includes("week")) {
            weekNumber = str.substring(str.lastIndexOf("week") + 4);
            weekNumber = Number(weekNumber);
        }
        //if (Number(weekNum) > $scope.currentWeekNo) {
        //    cellValue = 2
        //}
        //if (Number(weekNum) == $scope.currentWeekNo) {
        //    cellValue = 1
        //}
        //if (Number(weekNum) < $scope.currentWeekNo && Number(weekNum) > 0) {
        //    cellValue = 0
        //}
        //if (Number(weekNum) == 0) {
        //    cellValue = -1
        //}

        //return cellValue;
        return Number(weekNumber);
    }

    
    var n = new Date();
    var y = n.getFullYear();
    var m = n.getMonth() + 1;
    var d = n.getDate();
    $scope.currentDate = m + "/" + d + "/" + y;

    //Upload OSS Comments
    $scope.uploadOSSComments = function () {
        var condition = "update BotStatus set Status='Ready',ModifiedBy='"+$scope.userName+"',ModifiedDate=GETDATE() where BotName='OutputUpload'";
        Loadingstatus = "Uploading Comments.. <br/> This might take sometime.. <br/> Please check your mail.."
        start();
        postdata = JSON.stringify(
            {
                currentuser: $scope.userMailId,
                tablename: "BotStatus",
                task: "updateBot",
                condition: condition,
                setfields: "NA",
                user: $scope.userName,
                botname: "OutputUpload"
                //val: JSON.stringify(userData)
                //  strictmyview: strictmyview
            });
        $http({
            method: "POST",
            url: "/webservices/Manage.asmx/updateBotStatus",

            data: JSON.stringify({ data: postdata })
        }).then(function (results) {
            stop();
            //alert(JSON.stringify(results));
            if (results.data.d != null) {
                $scope.result = results.data.d;

                //alert("result" + JSON.stringify($scope.result));
                //$scope.MessageSuccess = $scope.result;
                $scope.info = true;
                color = Math.floor((Math.random() * 4) + 1);

                $.notify({
                    icon: "notifications",
                    message: results.data.d 

                });
                //$scope.info = true;
                //color = Math.floor((Math.random() * 4) + 1);

                //$.notify({
                //    icon: "notifications",
                //    message: $scope.result

                //}, {
                //    type: type[color],
                //    timer: 4000,
                //    placement: {
                //        from: from,
                //        align: align
                //    }
                //});

            }
            return results;
        }).catch(function (response) {
            stop();
            if (response.status === 408) {
                //alert("Error 408 :" + JSON.stringify(response));
                //$scope.MessageFailure = JSON.stringify(response);
                $scope.info = true;
                $.notify({
                    icon: "notifications",
                    message: JSON.stringify(response)

                });
            }
            else {
                // alert(JSON.stringify(response));
                //$scope.MessageFailure = JSON.stringify(response);
                $scope.info = true;
                $.notify({
                    icon: "notifications",
                    message: JSON.stringify(response)

                });
            }

        });;
    }

    function start() {
        var LoadingHover = "<div id='LoadAnimation'><span  style='font-size: 15px;font-weight: 500;align-items: center;'>" + Loadingstatus + "......</span><span style='align-content: center;'><img src='Images/Gear-1.6s-200px.gif' width='100px' /></span></div>"
        $("#LoadingMsg").html(LoadingHover);

        $(".LoadingM").css("display", "block")
    }
    function stop() {
        var LoadingHover = "<div id='LoadAnimation'><span  style='font-size: 15px;font-weight: 500;align-items: center;'>" + Loadingstatus + "......</span><span style='align-content: center;'><img src='Images/Gear-1.6s-200px.gif' width='100px' /></span></div>"
        $("#LoadingMsg").html(LoadingHover);

        $(".LoadingM").css("display", "none")
    }

    //User Input - Bulk Upload
    $scope.oncopy = function (copiedata) {
        //alert(copiedata);
        // $scope.copied.data = copiedata;
        $scope.copied.data = [];

        if (copiedata != null && copiedata != '' && copiedata != undefined) {
            // alert("if");
            $scope.ShowCopyText = true;
            var count = copiedata.split('\n').length;
            var tempLine = copiedata.split('\n');
            //     alert(count); alert(tempLine);
            // $scope.bulk = false;
            $("#copy").empty();
            var copied_Data = '<table border="1px">';
            var firstrow = 0;
            var coldef = [];

            //coldef.push('<input type="checkbox"/>');
            // $scope.copied.columnDefs = '[{field: "name", displayName: "Name",cellTemplate:"<input type="checkbox"/>"}]';
            // coldef.push('<input id="{{row.entity.OrderNo}}" style="margin-left: 15px;margin:11px; width:50%;" type="checkbox" value="{{row.entity.OrderNo}}" ng-checked="selection.indexOf(row.entity.OrderNo) > -1" ng-click="toggleSelection(row.entity.OrderNo)" />');
            for (var i = 0; i < count; i++) {
                var data = tempLine[i].split('\t');
                copied_Data += '<tr>';
                var tempobj = {};
                // coldef.push('<input id="{{row.entity.OrderNo}}" style="margin-left: 15px;margin:11px; width:50%;" type="checkbox" value="{{row.entity.OrderNo}}" ng-checked="selection.indexOf(row.entity.OrderNo) > -1" ng-click="toggleSelection(row.entity.OrderNo)" />');
                for (var j = 0; j < data.length; j++) {
                    copied_Data += '<td>' + data[j] + '</td>';
                    if (firstrow == 0) {
                        coldef.push(data[j]);
                    }
                    else {
                        tempobj[coldef[j]] = data[j];
                    }

                }
                if (firstrow != 0) $scope.copied.data.push(tempobj);

                firstrow++;
                copied_Data += '</tr>';
                //alert(JSON.stringify($scope.newdata));

            }
            copied_Data += '</table>';
            $("#copy").append(copied_Data);

            //$("#copy").show();
            //alert(JSON.stringify($scope.copied.data));
            angular.forEach($scope.copied.data, function (value, key) {

                if (key.indexOf('$') != 0)
                    $scope.columnschemaCopied.push({ field: key, displayName: key });
                else
                    $scope.columnschemaCopied.push({ field: key, displayName: key, visible: false });

            });
        }
        else {
            console.log("else");
        }


    }

    $scope.bulkUpload = function () {
        Loadingstatus = "Updating Comments"
        start();
        $scope.selectedCopiedRows = $scope.gridApi.selection.getSelectedRows();
       // alert(JSON.stringify($scope.selectedCopiedRows));
        //var condition = "set OSS_Comments='" + userData["FreeTextComments"] + "', ModifiedBy='" + $scope.userNtid + "', ModifiedDateTime=GETDATE() where SalesOrderId='" + userData["SAPOrdNo"] + "' and SalesOrderItemId='" + userData["ItemNo"] + "' and GoodsIssueQuantityEA='" + userData["AckedQuantity"] + "'";
        //alert(condition);
        postdata = JSON.stringify(
            {
                currentuser: $scope.userMailId,
                tablename: "UserDetails",
                task: "updateComments",
                //condition: condition,
                setfields: "NA",
                user: $scope.userName,
                //val: JSON.stringify($scope.copied.data)
                val: JSON.stringify($scope.selectedCopiedRows)
                //  strictmyview: strictmyview
            });
        $http({
            method: "POST",
            url: "/webservices/Manage.asmx/updateBacklogReport",

            data: JSON.stringify({ data: postdata })
        }).then(function (results) {
            //alert(JSON.stringify(results));
            if (results.data.d != null) {
                $('#UploadBulkComments').modal('toggle');
                $scope.result = results.data.d;
                stop();
                //alert("result" + JSON.stringify($scope.result));
                //$scope.MessageSuccess = $scope.result;
                $scope.info = true;
                color = Math.floor((Math.random() * 4) + 1);

                $.notify({
                    icon: "notifications",
                    message: results.data.d //+ " for " + userData["SAPOrdNo"]

                });
                
                $scope.uploadOSSComments();
            }
            return results;
        }).catch(function (response) {
            stop();
            if (response.status === 408) {
                //alert("Error 408 :" + JSON.stringify(response));
                //$scope.MessageFailure = JSON.stringify(response);
                $scope.info = true;
                $.notify({
                    icon: "notifications",
                    message: JSON.stringify(response)

                });
            }
            else {
                // alert(JSON.stringify(response));
                //$scope.MessageFailure = JSON.stringify(response);
                $scope.info = true;
                $.notify({
                    icon: "notifications",
                    message: JSON.stringify(response)

                });
            }

        });;
    }
});

var app1 = angular.module('OSSChart', ['chart.js','ui.grid', 'ui.grid.saveState', 'ui.grid.selection', 'ui.grid.cellNav', 'ui.grid.resizeColumns', 'ui.grid.moveColumns']);
app1.controller('OSSCharts', function ($scope, $http, $sce) {
    //alert("Hi!");
    $scope.outputCountDetails = {
        //data: alldata,
        enableFiltering: true,
        showGridFooter: true,
        //columnDefs: $scope.columnschemaPO
        columnDefs: [
            //{ name: 'edit', displayName: 'Edit', cellTemplate: '<button id="editBtn" type="button" class="md-fab md-warn md-primary md-raised btn btn-small" style="padding:0px 12px;margin:5px 5px;background-color: #ffffff00;" data-toggle="modal" data-target="#editComments" ng-click="grid.appScope.edit(row.entity)" ><i class="fa fa-pencil-square-o"></i></button>', width: 60, enableCellEdit: false },
            { field: 'FreeTextComments', displayName: 'FreeTextComments', width: 300 },
            { field: 'CommentsCount', displayName: 'Count', width: 69 },
        ]
    };
    $scope.getUser = function () {
        $http({
            method: "POST",
            url: "/getCurrentUser.asmx/getUserLDAP",
            data: JSON.stringify({ data: '' })
        }).then(function (results) {
         //   console.log(JSON.stringify(results));
         //   console.log(results.data.d.NtID);
            $scope.userNtid = results.data.d.NtID;
            $scope.userName = results.data.d.FullName;
            $scope.userMailId = results.data.d.WorkEmail;
            return results;
        }).catch(function (response) {
            if (response.status === 408) {
                // alert("408: Error Loading Catalog" + JSON.stringify(response), "Error", "fail");

                $scope.info = true;
                $.notify({
                    icon: "notifications",
                    message: JSON.stringify(response)

                });

            }
            else {
                // alert("Error Loading Catalog" + JSON.stringify(response), "Error", "fail");

                $scope.info = true;
                $.notify({
                    icon: "notifications",
                    message: JSON.stringify(response)

                });

            }

        });
    };

    $scope.getUser();
    $scope.loadOutput = function (rec) {
        Loadingstatus = "Loading the OSS Output"
        start();
        postdata = JSON.stringify(
            {
                //currentuser: $scope.userNtid,
                tablename: "OSS_Output_View",
                task: "CommentsCount",
                condition: "",
                setfields: "NA"
                //  strictmyview: strictmyview
            });
        $http({
            method: "POST",
            url: "/webservices/Manage.asmx/retrieveBacklog",

            data: JSON.stringify({ data: postdata })
        }).then(function (results) {
            //console.log("Hi"+JSON.stringify(results));
            if (results.data.d.rowData != null) {

                $scope.outputCountDetails.data = results.data.d.rowData;
                $scope.Comments = [];
                $scope.Count = [];
                //$.each($.parseJSON($scope.outputdetails), function (k, v) {
                //    alert(k + ' is ' + v);
                //    if(k == "FreeTextComments")
                //        $scope.Comments.push(v);
                //    else if (k == "CommentsCount")
                //        $scope.Count.push(v);
                //});

                angular.forEach(results.data.d.rowData, function (value, key) {
                        $scope.Comments.push(value["FreeTextComments"]);
                        $scope.Count.push(value["CommentsCount"]);

                });

                // alert("output " + JSON.stringify($scope.outputdetails.data));
                //stop();
                $scope.labels = $scope.Comments;
                $scope.data = $scope.Count;
            }
            stop();
            return results;
        }).catch(function (response) {
            if (response.status === 408) {
                //alert("Error 408 :"+JSON.stringify(response));

                $scope.info = true;
                $.notify({
                    icon: "notifications",
                    message: "Error 408 :" + JSON.stringify(response)

                });
                // Model.errorMessage("Error 408 : Error loading POs" + JSON.stringify(response), "Error", "fail");
                stop();
            }
            else {
                //alert(JSON.stringify(response));
                $scope.info = true;
                $.notify({
                    icon: "notifications",
                    message: JSON.stringify(response)

                });
                //Model.errorMessage("Error loading POs" + JSON.stringify(response), "Error", "fail");
                stop();
            }

        });;
    }

    $scope.loadOutput();

    //$scope.labels = ["Download Sales", "In-Store Sales", "Mail-Order Sales"];
    //$scope.data = [300, 500, 100];

    function start() {
        var LoadingHover = "<div id='LoadAnimation'><span  style='font-size: 15px;font-weight: 500;align-items: center;'>" + Loadingstatus + "......</span><span style='align-content: center;'><img src='Images/Gear-1.6s-200px.gif' width='100px' /></span></div>"
        $("#LoadingMsg").html(LoadingHover);

        $(".LoadingM").css("display", "block")
    }
    function stop() {
        var LoadingHover = "<div id='LoadAnimation'><span  style='font-size: 15px;font-weight: 500;align-items: center;'>" + Loadingstatus + "......</span><span style='align-content: center;'><img src='Images/Gear-1.6s-200px.gif' width='100px' /></span></div>"
        $("#LoadingMsg").html(LoadingHover);

        $(".LoadingM").css("display", "none")
    }
});
$(document).ready(function () {
   // console.log("jquery ready");
    //console.log($("#blrdata").val());
    $(".close").on("click", function () {
        $(".popout").css("display", "none");
    });

    $('a.tabs').click(function () {
        var id = $(this).attr('href');
        //  alert(id);
        $(id).fadeIn('slow', function () {
            // Animation complete
        });

        $(".tab-pane").hide();
        $(id).show();
    });


});

// Example starter JavaScript for disabling form submissions if there are invalid fields
(function () {
    'use strict';
    window.addEventListener('load', function () {
        // Fetch all the forms we want to apply custom Bootstrap validation styles to
        var forms = document.getElementsByClassName('needs-validation');
        // Loop over them and prevent submission
        var validation = Array.prototype.filter.call(forms, function (form) {
            form.addEventListener('submit', function (event) {
                if (form.checkValidity() === false) {
                    event.preventDefault();
                    event.stopPropagation();
                }
                form.classList.add('was-validated');
            }, false);
        });
    }, false);
})();

