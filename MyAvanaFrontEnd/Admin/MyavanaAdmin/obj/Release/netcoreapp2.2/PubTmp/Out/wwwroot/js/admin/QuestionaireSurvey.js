
        $(document).ready(function () {
          var list =   $('.js-example-basic-multiple').select2({
                placeholder: "-- Select --",
              closeOnSelect: false
            }).on("select2:closing", function (e) {
               // e.preventDefault();
            }).on("select2:closed", function (e) {
               // list.select2("open");
              }).on('select2:opening', function (e) {
                  $(window).scrollTop(0);
                  var scrollTop = $(window).scrollTop();
                  var y = $(window).scrollTop();
                  if (scrollTop + $(window).innerHeight() >= this.scrollHeight) {
                      $(window).scrollTop(y + 1);
                  } else if (scrollTop <= 0) {
                      $(window).scrollTop(y - 1);
                  }

              });

            $('.maxThree').select2({
                placeholder: "-- Select --",
                closeOnSelect: false,
                maximumSelectionLength: 3
            }).on("select2:closing", function (e) {
                // e.preventDefault();
            }).on("select2:closed", function (e) {
                // list.select2("open");
            }).on('select2:opening', function (e) {
                $(window).scrollTop(0);
                var scrollTop = $(window).scrollTop();
                var y = $(window).scrollTop();
                if (scrollTop + $(window).innerHeight() >= this.scrollHeight) {
                    $(window).scrollTop(y + 1);
                } else if (scrollTop <= 0) {
                    $(window).scrollTop(y - 1);
                }
            })
        });

        var questionId = 0;
        var answerId = 0;
        var parentValue;
        var questionaire = [];
        var dataList = {};
        var arr29 = [];
        var arr7 = [];
        var arr9 = [];
        var arr10 = [];
        var arr14 = [];
        var arr15 = [];
        var arr16 = [];
        $('.1').click(function () {

            dataList = {};
            answerId = $("input[name='1']:checked").val();
            dataList.questionId = 1;
            dataList.answerId = answerId;
            if ($('#isDigitalAnalysis').val() != "1") {
                $("#nxt_1").attr("hidden", false);
            }
            else {
                $("#nxt_3").attr("hidden", false);
            }

            $.each(questionaire, function (i, el) {
                if (this.questionId == 1) {
                    questionaire.splice(i, 1);
                }
            });
            questionaire.push(dataList);
        });

        function HasQuestion(questionId) {
            var IsQuestion = false;
            $.each(questionaire, function (i, el) {
                if (this.questionId == questionId) {
                    IsQuestion = true;
                    return false;
                }
            });
            return IsQuestion;
        }

        $('.30').click(function () {
            dataList = {};
            var id = $("input[name='30']:checked").val();
            var findQuestion = questionaire.findIndex(x => x.questionId === 30);
            if (findQuestion >= 0) {
                questionaire.splice(findQuestion, 1);
            }
            let check = $("#" + id).is(":checked") ? "true" : "false";
            if (check == "true") {
                if (id == "112") {
                    $("#txt_30").css("display", "block");
                    $("#txt_30").val("");
                    $("#nxt_30").attr("hidden", true);
                } else {
                    $("#txt_30").css("display", "none");
                    dataList.questionId = 30;
                    answerId = $("input[name='30']:checked").val();
                    dataList.answerId = $("input[name='30']:checked").val();
                    $("#nxt_30").attr("hidden", false);
                    questionaire.push(dataList);
                }
            } else {
                //var IsQuestion = "0";
                var IsQuestion = HasQuestion(30);
                if (!IsQuestion)
                    $("#nxt_30").attr("hidden", true);
            }
        });

        $('.2').click(function () {

            dataList = {};
            var id = $(this).val();
            let check = $("#radio" + id).is(":checked") ? "true" : "false";
            if (check == "true") {
                dataList.questionId = 2;
                answerId = $("input[name='2']:checked").val();
                dataList.answerId = id;
                $("#nxt_2").attr("hidden", false);
                questionaire.push(dataList);
            } else {
                $.each(questionaire, function (i, el) {
                    if (this.answerId == id) {
                        questionaire.splice(i, 1);
                    }
                });
                //var IsQuestion = "0";
                var IsQuestion = HasQuestion(2);
                if (!IsQuestion)
                    $("#nxt_2").attr("hidden", true);
            }
        });
        $('.3').click(function () {
            dataList = {};
            var id = $(this).val();
            let check = $("#radio" + id).is(":checked") ? "true" : "false";
            if (check == "true") {
                dataList.questionId = 3;
                //dataList.answerId = $("input[name='3']:checked").val();
                dataList.answerId = $(this).val();
                $("#nxt_3").attr("hidden", false);
                questionaire.push(dataList);
            } else {
                $.each(questionaire, function (i, el) {
                    if (this.answerId == id) {
                        questionaire.splice(i, 1);
                    }
                });
                var IsQuestion = HasQuestion(3);
                if (!IsQuestion)
                    $("#nxt_3").attr("hidden", true);

                //let findArray = questionaire.filter(item => !forDeletion.includes(item))
            }
        });
        $('.4').click(function () {

            dataList = {};
            answerId = $("input[name='4']:checked").val();
            dataList.questionId = 4;
            dataList.answerId = answerId;
            $("#nxt_4").attr("hidden", false);
            $.each(questionaire, function (i, el) {
                if (this.questionId == 4) {
                    questionaire.splice(i, 1);
                }
            });
            questionaire.push(dataList);
        });
        $('.5').click(function () {

            dataList = {};
            answerId = $("input[name='5']:checked").val();
            dataList.questionId = 5;
            dataList.answerId = answerId;
            $("#nxt_5").attr("hidden", false);
            $.each(questionaire, function (i, el) {
                if (this.questionId == 5) {
                    questionaire.splice(i, 1);
                }
            });
            questionaire.push(dataList);
        });

        $('.8').click(function () {
            dataList = {};
            answerId = $("input[name='8']:checked").val();
            dataList.questionId = 8;
            dataList.answerId = answerId;
            $("#nxt_8").attr("hidden", false);
            $.each(questionaire, function (i, el) {
                if (this.questionId == 8) {
                    questionaire.splice(i, 1);
                }
            });
            questionaire.push(dataList);
        });
        $('.10').click(function () {
            dataList = {};
            answerId = $("input[name='10']:checked").val();
            dataList.questionId = 10;
            dataList.answerId = answerId;
            $("#nxt_10").attr("hidden", false);
            $.each(questionaire, function (i, el) {
                if (this.questionId == 10) {
                    questionaire.splice(i, 1);
                }
            });
            questionaire.push(dataList);
        });

        //$('#9').change(function () {
        //    dataList = { };
        //    var id = $(this).val();

        //    $.each(questionaire, function (i, el) {
        //        if (this.questionId == 9) {
        //            questionaire.splice(i, 1);
        //        }
        //        $("#nxt_9").attr("hidden", true);
        //    });

        //    if (id != 0) {
        //        dataList.questionId = 9;
        //        dataList.answerId = id;
        //        $("#nxt_9").attr("hidden", false);
        //        questionaire.push(dataList);
        //    }
        //});

        //$('#10').change(function () {

        //    dataList = { };
        //    var id = $(this).val();

        //    $.each(questionaire, function (i, el) {
        //        if (this.questionId == 10) {
        //            questionaire.splice(i, 1);
        //        }
        //        $("#nxt_10").attr("hidden", true);
        //    });

        //    if (id != 0) {
        //        dataList.questionId = 10;
        //        answerId = $(this).val();
        //        dataList.answerId = id;
        //        $("#nxt_10").attr("hidden", false);
        //        questionaire.push(dataList);
        //    }
        //});


        //$('#32').change(function () {

        //    let answerId = $('#32 option:selected').val();
        //    if (answerId == 0) {
        //        $("#nxt_32").attr("hidden", true);
        //        return false;
        //    }
        //    dataList = { };
        //    dataList.questionId = 32;
        //    dataList.answerId = answerId;
        //    questionaire.push(dataList);
        //    $("#nxt_32").attr("hidden", false);
        //});

        var elements = [];
    $('#14').change(function () {
            var array = $(this).val();
    if (array == undefined) {
        $("#nxt_14").attr("hidden", true);
    return false;
            }

    $.each(array, function (i, l) {
        $.each(questionaire, function (i, el) {
            if (this.questionId == 14) {
                questionaire.splice(i, 1);
            }
        });
            });

    $.each(array, function (i, l) {
        dataList = {};
    dataList.questionId = 14;
    dataList.answerId = l;
    questionaire.push(dataList);
    $("#nxt_14").attr("hidden", false);
            });
        });

    $('#15').change(function () {

            var array = $(this).val();
    if (array == undefined) {
        $("#nxt_15").attr("hidden", true);
    return false;
            }

    $.each(array, function (i, l) {
        $.each(questionaire, function (i, el) {
            if (this.questionId == 15) {
                questionaire.splice(i, 1);
            }
        });
            });

    $.each(array, function (i, l) {
        dataList = {};
    dataList.questionId = 15;
    dataList.answerId = l;
    questionaire.push(dataList);
    $("#nxt_15").attr("hidden", false);
            });
        });

    $('#drp37').change(function () {

            var array = $(this).val();
    if (array == undefined) {
        $("#nxt_37").attr("hidden", true);
    return false;
            }

    $.each(array, function (i, l) {
        $.each(questionaire, function (i, el) {
            if (this.questionId == 37) {
                questionaire.splice(i, 1);
            }
        });
            });

    $.each(array, function (i, l) {
        dataList = {};
    dataList.questionId = 37;
    dataList.answerId = l;
    questionaire.push(dataList);
    if ($('#isDigitalAnalysis').val() != "1") {
        $("#nxt_37").attr("hidden", false);
                }
    else {
        $("#nxt_22").attr("hidden", false);
                }

            });
        });

    $('#16').change(function () {

            var array = $(this).val();
    if (array == undefined) {
        $("#nxt_16").attr("hidden", true);
    return false;
            }

    $.each(array, function (i, l) {
        $.each(questionaire, function (i, el) {
            if (this.questionId == 16) {
                questionaire.splice(i, 1);
            }
        });
            });

    $.each(array, function (i, l) {
        dataList = {};
    dataList.questionId = 16;
    dataList.answerId = l;
    questionaire.push(dataList);
    $("#nxt_16").attr("hidden", false);
            });

        });

    $('.17').click(function () {

        dataList = {};
    answerId = $("input[name='17']:checked").val();
    dataList.questionId = 17;
    dataList.answerId = answerId;
    $("#nxt_17").attr("hidden", false);
    $.each(questionaire, function (i, el) {
                if (this.questionId == 17) {
        questionaire.splice(i, 1);
                }
            });
    questionaire.push(dataList);
        });

    $('#drp31').change(function () {

            var array = $(this).val();
    if (array == undefined) {
        $("#nxt_31").attr("hidden", true);
    return false;
            }

    $.each(array, function (i, l) {
        $.each(questionaire, function (i, el) {
            if (this.questionId == 31) {
                questionaire.splice(i, 1);
            }
        });
            });

    $.each(array, function (i, l) {
        dataList = {};
    dataList.questionId = 31;
    dataList.answerId = l;
    questionaire.push(dataList);
    $("#nxt_31").attr("hidden", false);
            });

        });
    $('#33').change(function () {

            var array = $(this).val();
    if (array == undefined) {
        $("#nxt_33").attr("hidden", true);
    return false;
            }

    $.each(array, function (i, l) {
        $.each(questionaire, function (i, el) {
            if (this.questionId == 33) {
                questionaire.splice(i, 1);
            }
        });
            });

    $.each(array, function (i, l) {
        dataList = {};
    dataList.questionId = 33;
    dataList.answerId = l;
    questionaire.push(dataList);
    $("#nxt_33").attr("hidden", false);
            });

        });

    $('.18').click(function () {
        dataList = {};
    var id = $(this).val();
    let check = $("#radio" + id).is(":checked") ? "true" : "false";
    if (check == "true") {
        dataList.questionId = 18;
    answerId = $(this).val();
    dataList.answerId = id;
    $("#nxt_18").attr("hidden", false);
    questionaire.push(dataList);
            } else {
        $.each(questionaire, function (i, el) {
            if (this.answerId == id) {
                questionaire.splice(i, 1);
            }
        });
    //var IsQuestion = "0";
    var IsQuestion = HasQuestion(18);
    if (!IsQuestion)
    $("#nxt_18").attr("hidden", true);
            }
        });

    $('.24').click(function () {

        dataList = {};
    answerId = $("input[name='24']:checked").val();
    dataList.questionId = 24;
    dataList.answerId = answerId;
    $("#nxt_24").attr("hidden", false);
    $.each(questionaire, function (i, el) {
                if (this.questionId == 24) {
        questionaire.splice(i, 1);
                }
            });
    questionaire.push(dataList);
        });
    $('.33').click(function () {

        dataList = {};
    answerId = $("input[name='33']:checked").val();
    dataList.questionId = 33;
    dataList.answerId = answerId;
    $("#nxt_33").attr("hidden", false);
    $.each(questionaire, function (i, el) {
                if (this.questionId == 33) {
        questionaire.splice(i, 1);
                }
            });
    questionaire.push(dataList);
        });
    $('.32').click(function () {

        dataList = {};
    answerId = $("input[name='32']:checked").val();
    dataList.questionId = 32;
    dataList.answerId = answerId;
    $("#nxt_32").attr("hidden", false);
    $.each(questionaire, function (i, el) {
                if (this.questionId == 32) {
        questionaire.splice(i, 1);
                }
            });
    questionaire.push(dataList);
        });
    $('.7').click(function () {

        dataList = {};
    answerId = $("input[name='7']:checked").val();
    dataList.questionId = 7;
    dataList.answerId = answerId;
    $("#nxt_7").attr("hidden", false);
    $.each(questionaire, function (i, el) {
                if (this.questionId == 7) {
        questionaire.splice(i, 1);
                }
            });
    questionaire.push(dataList);
        });
    $('.6').click(function () {

        dataList = {};
    answerId = $("input[name='6']:checked").val();
    dataList.questionId = 6;
    dataList.answerId = answerId;
    $("#nxt_6").attr("hidden", false);
    $.each(questionaire, function (i, el) {
                if (this.questionId == 6) {
        questionaire.splice(i, 1);
                }
            });
    questionaire.push(dataList);
        });
    $('.9').click(function () {

        dataList = {};
    answerId = $("input[name='9']:checked").val();
    dataList.questionId = 9;
    dataList.answerId = answerId;
    $("#nxt_9").attr("hidden", false);
    $.each(questionaire, function (i, el) {
                if (this.questionId == 9) {
        questionaire.splice(i, 1);
                }
            });
    questionaire.push(dataList);
        });
    $('#drp25').change(function () {

            var array = $(this).val();
    if (array == undefined) {
        $("#nxt_25").attr("hidden", true);
    return false;
            }

    $.each(array, function (i, l) {
        $.each(questionaire, function (i, el) {
            if (this.questionId == 25) {
                questionaire.splice(i, 1);
            }
        });
            });

    $.each(array, function (i, l) {
        dataList = {};
    dataList.questionId = 25;
    dataList.answerId = l;
    questionaire.push(dataList);
    $("#nxt_25").attr("hidden", false);
            });
        });

    $('#drp19').change(function () {
            var array = $(this).val();
    if (array == undefined) {
        $("#nxt_19").attr("hidden", true);
    return false;
            }

    $.each(array, function (i, l) {
        $.each(questionaire, function (i, el) {
            if (this.questionId == 19) {
                questionaire.splice(i, 1);
            }
        });
            });

    $.each(array, function (i, l) {
        dataList = {};
    dataList.questionId = 19;
    dataList.answerId = l;
    questionaire.push(dataList);
    $("#nxt_19").attr("hidden", false);
            });
        });


    $('#11').change(function () {

            var array = $(this).val();
    if (array == undefined) {
        $("#nxt_11").attr("hidden", true);
    return false;
            }

    $.each(array, function (i, l) {
        $.each(questionaire, function (i, el) {
            if (this.questionId == 11) {
                questionaire.splice(i, 1);
            }
        });
            });

    $.each(array, function (i, l) {
        dataList = {};
    dataList.questionId = 11;
    dataList.answerId = l;
    questionaire.push(dataList);
    $("#nxt_11").attr("hidden", false);
            });
        });

    $('#28').change(function () {

            var array = $(this).val();
    let check = $(this).is(":checked") ? "true" : "false";
    if (array == undefined) {
        $("#nxt_28").attr("hidden", true);
    return false;
            }

    $.each(array, function (i, l) {
        $.each(questionaire, function (i, el) {
            if (this.questionId == 28) {
                questionaire.splice(i, 1);
            }
        });
            });

    $.each(array, function (i, l) {
        dataList = {};
    dataList.questionId = 28;
    dataList.answerId = l;
    questionaire.push(dataList);
    $("#nxt_28").attr("hidden", false);
            });

        });

        //$('#32').change(function () {

        //    dataList = {};
        //    var id = $(this).val();

        //    $.each(questionaire, function (i, el) {
        //        if (this.questionId == 32) {
        //            questionaire.splice(i, 1);
        //        }
        //        $("#nxt_32").attr("hidden", true);
        //    });

        //    if (id != 0) {
        //        dataList.questionId = 32;
        //        answerId = $(this).val();
        //        dataList.answerId = id;
        //        $("#nxt_32").attr("hidden", false);
        //        questionaire.push(dataList);
        //    }

        //});

        //$('#7').change(function () {

        //    dataList = {};
        //    var id = $(this).val();

        //    $.each(questionaire, function (i, el) {
        //        if (this.questionId == 7) {
        //            questionaire.splice(i, 1);
        //        }
        //        $("#nxt_7").attr("hidden", true);
        //    });

        //    if (id != 0) {
        //        dataList.questionId = 7;
        //        answerId = $(this).val();
        //        dataList.answerId = id;
        //        $("#nxt_7").attr("hidden", false);
        //        questionaire.push(dataList);
        //    }
        //});

        //$('#6').change(function () {

        //    dataList = {};
        //    var id = $(this).val();

        //    $.each(questionaire, function (i, el) {
        //        if (this.questionId == 6) {
        //            questionaire.splice(i, 1);
        //        }
        //        $("#nxt_6").attr("hidden", true);
        //    });

        //    if (id != 0) {
        //        dataList.questionId = 6;
        //        answerId = $(this).val();
        //        dataList.answerId = id;
        //        $("#nxt_6").attr("hidden", false);
        //        questionaire.push(dataList);
        //    }
        //});

        function returnInputError(id) {
            if (id == 11) {
                if ($("#txt_11").val() != "") {
                    $("#nxt_11").attr("hidden", false);
                } else {
                    $("#nxt_11").attr("hidden", true);
                }
            }
            if (id == 12) {
                if ($("#txt_12").val() != "") {
                    $("#nxt_12").attr("hidden", false);
                } else {
                    $("#nxt_12").attr("hidden", true);
                }
            }
            if (id == 13) {
                if ($("#txt_13").val() != "") {
                    $("#nxt_13").attr("hidden", false);
                } else {
                    $("#nxt_13").attr("hidden", true);
                }
            }
            if (id == 19) {
                if ($("#txt_19").val() != "") {
                    $("#nxt_19").attr("hidden", false);
                } else {
                    $("#nxt_19").attr("hidden", true);
                }
            }
            if (id == 20) {
                if ($("#txt_20").val() != "") {
                    $("#nxt_20").attr("hidden", false);
                } else {
                    $("#nxt_20").attr("hidden", true);
                }
            }
            if (id == 21) {
                if ($("#txt_21").val() != "") {
                    $("#nxt_21").attr("hidden", false);
                } else {
                    $("#nxt_21").attr("hidden", true);
                }
            }
            if (id == 35) {
                if ($("#txt_35").val() != "") {
                    $("#nxt_35").attr("hidden", false);
                } else {
                    $("#nxt_35").attr("hidden", true);
                }
            }
            if (id == 36) {
                if ($("#txt_36").val() != "") {
                    $("#nxt_36").attr("hidden", false);
                } else {
                    $("#nxt_36").attr("hidden", true);
                }
            }
            if (id == 26) {
                if ($("#txt_26").val() != "") {
                    $("#nxt_26").attr("hidden", false);
                } else {
                    $("#nxt_26").attr("hidden", true);
                }
            }
            if (id == 38) {
                if ($("#txt_38").val() != "") {
                    $("#nxt_38").attr("hidden", false);
                } else {
                    $("#nxt_38").attr("hidden", true);
                }
            }
            if (id == 27) {
                if ($("#txt_27").val() != "") {
                    $("#nxt_27").attr("hidden", false);
                } else {
                    $("#nxt_27").attr("hidden", true);
                }
            }
            if (id == 30) {
                if ($("#txt_30").val() != "") {
                    $("#nxt_30").attr("hidden", false);
                } else {
                    $("#nxt_30").attr("hidden", true);
                }
            }
        }

        function hideShowButton(event) {
            if ($('#txt_29').val().trim() || $('#txt_30').val().trim() || $('#txt_31').val().trim() || $('#txt_32').val().trim()) {
        $("#nxt_29").attr("hidden", false);
            } else {
        $("#nxt_29").attr("hidden", true);
            }
        }

    function txtResponse(id) {
            if (id == 38) {
        dataList = {};
    dataList.questionId = id;
    dataList.descriptiveAnswer = $("#txt_38").val();
    questionaire.push(dataList);
            }
    if (id == 11) {
        dataList = {};
    dataList.questionId = id;
    dataList.descriptiveAnswer = $("#txt_11").val();
    questionaire.push(dataList);
            } if (id == 12) {
        dataList = {};
    dataList.questionId = id;
    dataList.descriptiveAnswer = $("#txt_12").val();
    questionaire.push(dataList);
            } if (id == 13) {
        dataList = {};
    dataList.questionId = id;
    dataList.descriptiveAnswer = $("#txt_13").val();
    questionaire.push(dataList);
            } if (id == 19) {
        dataList = {};
    dataList.questionId = id;
    dataList.descriptiveAnswer = $("#txt_19").val();
    questionaire.push(dataList);
            } if (id == 20) {
        dataList = {};
    dataList.questionId = id;
    dataList.descriptiveAnswer = $("#txt_20").val();
    questionaire.push(dataList);
            }
    if (id == 21) {
        dataList = {};
    dataList.questionId = id;
    dataList.descriptiveAnswer = $("#txt_21").val();
    questionaire.push(dataList);
            }

    if (id == 30 && $("#txt_30").css("display") == "block") {
                var findQuestion = questionaire.findIndex(x => x.questionId === 30);
                if (findQuestion >= 0) {
        questionaire.splice(findQuestion, 1);
                }
    dataList = { };
    dataList.questionId = id;
    dataList.answerId = $("input[name='30']:checked").val();
    dataList.descriptiveAnswer = $("#txt_30").val();
    questionaire.push(dataList);
            }
    if (id == 26) {
        dataList = {};
    dataList.questionId = id;
    dataList.descriptiveAnswer = $("#txt_26").val();
    questionaire.push(dataList);

    // due to new design
    dataList = { };
    dataList.questionId = 38;
    dataList.descriptiveAnswer = $("#txt_38").val();
    questionaire.push(dataList);
            }
    if (id == 27) {
        dataList = {};
    dataList.questionId = id;
    dataList.descriptiveAnswer = $("#txt_27").val();
    questionaire.push(dataList);
            }

    if (id == 35) {
        dataList = {};
    dataList.questionId = id;
    dataList.descriptiveAnswer = $("#txt_35").val();
    questionaire.push(dataList);
            }

    if (id == 36) {
        dataList = {};
    dataList.questionId = id;
    dataList.descriptiveAnswer = $("#txt_36").val();
    questionaire.push(dataList);
            }



    if (id == 29) {
                if ($("#txt_29").val() != "") {
        dataList = {};
    dataList.questionId = id;
    dataList.descriptiveAnswer = $("#txt_29").val();
    questionaire.push(dataList);
                }
    if ($("#txt_30").val() != "") {
        dataList = {};
    dataList.questionId = id;
    dataList.descriptiveAnswer = $("#txt_30").val();
    questionaire.push(dataList);
                }
    if ($("#txt_31").val() != "") {
        dataList = {};
    dataList.questionId = id;
    dataList.descriptiveAnswer = $("#txt_31").val();
    questionaire.push(dataList);
                }
    if ($("#txt_32").val() != "") {
        dataList = {};
    dataList.questionId = id;
    dataList.descriptiveAnswer = $("#txt_32").val();
    questionaire.push(dataList);
                }
            }
        }

    var fdata = new FormData();
    var dragdrop = {
        drop: function (event) {

        event.preventDefault();
    fdata.delete("file");
    fdata = new FormData();
    var files = event.dataTransfer.files;
    for (var i = 0; i < files.length; i++) {
        fdata.append("file", files[i]);
                }
    $("#dragHere").prop("hidden", true);
    $("#uploadedImage").prop("hidden", false);

    $('#imageShowing').attr('src', "");
    imageUpload();
                //$("#nxt_22").attr("hidden", false);
            },
    drag: function (event) {
        event.preventDefault();
            }
        };

    $('#changeImage').click(function () {
        $("#uploadedImage").prop("hidden", true);
    $("#dragHere").prop("hidden", false);
    $("#nxt_22").attr("hidden", true);

    $('#imageShowing').attr('src', "");
        });

    $("#hairImage").change(function () {
        fdata = new FormData();

    var file = $('#hairImage').get(0).files[0];
    fdata.append("file", file);
    $("#dragHere").prop("hidden", true);
    $("#uploadedImage").prop("hidden", false);

    $('#imageShowing').attr('src', "");
    imageUpload();
            //$("#nxt_22").attr("hidden", false);
        });
    var filename;
    function imageUpload() {
        $.ajax({
            //dataType: 'json',
            type: "POST",
            url: "/Questionnaire/SaveImage",
            data: fdata,
            contentType: false,
            processData: false,
            success: function (response) {

                if (response != undefined) {
                    $('#imageShowing').attr('src', "/questionnaireFile/" + response)
                    $('#hairImage').val('');
                    filename = response;
                    $("#nxt_22").attr("hidden", false);
                } else {

                }
            },
        });
        }
    var getUrlParameter = function getUrlParameter(sParam) {
            var sPageURL = window.location.search.substring(1),
    sURLVariables = sPageURL.split('&'),
    sParameterName,
    i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

    if (sParameterName[0] === sParam) {
                    return sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
                }
            }
    return false;
        };
    //AJAX to save questionaire data
    var formData = new FormData();
    function saveQuestionaire(id) {
        $('#successMessage2').text("Cloud Analyzing Questionnaire Responses....");
    $('.alert-success2').css("display", "block");

    //var file = $('#hairImage').get(0).files[0];
    dataList = { };
    dataList.questionId = id;

    var aa ='@ViewBag.check'+'a';

    if (getUrlParameter('qa')) {
        dataList.QA = getUrlParameter('qa');
            } else {
        dataList.QA = 0;
            }
    dataList.descriptiveAnswer = filename;
    dataList.UserId = $("#UserId").val();
    questionaire.push(dataList);

            //$.each(questionaire, function (i, el) {
        //    if (this.answerId == "null") {
        //        questionaire.splice(i, 1);
        //    }
        //});
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: "POST",
            url: "/Questionnaire/SaveQuestionaire",
            data: JSON.stringify(questionaire),

            success: function (response) {
                if (response == "1") {

                    // $('#t-2').addClass('blur_background');
                    setTimeout(function () {
                        $('.alert-success2').fadeOut();
                        $("#myModal").modal({ backdrop: 'static', keyboard: false });
                    }, 3000)

                    //window.location.href = "/Questionaire/DigitalAssessment";
                } else {
                    window.location.href = "/Auth/login";
                }
            },
        });
        }

    function saveQuestionaire_hairKit(id) {

            var file = $('#hairImage').get(0).files[0];
    dataList = { };
    dataList.questionId = id;

    var aa ='@ViewBag.check'+'a';

    if (getUrlParameter('qa')) {
        dataList.QA = getUrlParameter('qa');
            } else {
        dataList.QA = 0;
            }
    dataList.descriptiveAnswer = filename;
    dataList.UserId = $("#UserId").val();
    questionaire.push(dataList);

            //$.each(questionaire, function (i, el) {
        //    if (this.answerId == "null") {
        //        questionaire.splice(i, 1);
        //    }
        //});
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: "POST",
            url: "/Questionnaire/SaveQuestionaire",
            data: JSON.stringify(questionaire),

            success: function (response) {
                if (response == "1") {
                    // window.location.href = "/HairProfile/CustomerHair";
                } else {
                    window.location.href = "/Auth/login";
                }
            },
        });
        }

    $("#myModal").on("hidden.bs.modal", function () {
        $('#t-2').removeClass('blur_background');
        });
    function redirectToDigitalAssessment() {
        window.location.href = "/Questionnaire/DigitalAssessment";
        }

        (function ($) {
            var elActive = '';
        $.fn.selectCF = function (options) {
                // option
                var settings = $.extend({
            color: "rgb(112, 112, 112)", // color
        backgroundColor: "#none", // background
        change: function () { }, // event change
                }, options);

        return this.each(function () {

                    var selectParent = $(this);
        list = [],
        html = '';

        //parameter CSS
        var width = $(selectParent).width();

        $(selectParent).hide();
        if ($(selectParent).children('option').length == 0) { return; }
        $(selectParent).children('option').each(function () {
                        if ($(this).is(':selected')) {s = 1; title = $(this).text(); } else {s = 0; }
        list.push({
            value: $(this).attr('value'),
        text: $(this).text(),
        selected: s,
                        })
                    })

        // style
        var style = " background: " + settings.backgroundColor + "; color: " + settings.color + " ";

        html += "<ul class='selectCF'>";
            html += "<li>";

                html += "<span class='titleCF' >" + title + "</span>";
                html += "<span class='searchCF' style='" + style + "; width:" + width + "px'><input style='color:" + settings.color + "' /></span>";
                html += "<span class='arrowCF ion-chevron-right' style='" + style + "'></span>";
                html += "<ul>";
                    $.each(list, function (k, v) {
                        s = (v.selected == 1) ? "selected" : "";
                    html += "<li value=" + v.value + " class='" + s + "'>" + v.text + "</li>";
                    })
                    html += "</ul>";
                html += "</li>";
            html += "</ul>";
        $(selectParent).after(html);
        var customSelect = $(this).next('ul.selectCF'); // add Html
        var seachEl = $(this).next('ul.selectCF').children('li').children('.searchCF');
        var seachElOption = $(this).next('ul.selectCF').children('li').children('ul').children('li');
        var seachElInput = $(this).next('ul.selectCF').children('li').children('.searchCF').children('input');

        // handle active select
        $(customSelect).unbind('click').bind('click', function (e) {
            e.stopPropagation();
        if ($(this).hasClass('onCF')) {
            elActive = '';
        $(this).removeClass('onCF');
        $(this).removeClass('searchActive'); $(seachElInput).val('');
        $(seachElOption).show();
                        } else {
                            if (elActive != '') {
            $(elActive).removeClass('onCF');
        $(elActive).removeClass('searchActive'); $(seachElInput).val('');
        $(seachElOption).show();
                            }
        elActive = $(this);
        $(this).addClass('onCF');
        $(seachEl).children('input').focus();
                        }
                    })

        // handle choose option
        var optionSelect = $(customSelect).children('li').children('ul').children('li');
        $(optionSelect).bind('click', function (e) {
                        var value = $(this).attr('value');
        if ($(this).hasClass('selected')) {
            //
        } else {
            $(optionSelect).removeClass('selected');
        $(this).addClass('selected');
        $(customSelect).children('li').children('.titleCF').html($(this).html());
        $(selectParent).val(value);
        settings.change.call(selectParent); // call event change
                        }
                    })

        // handle search
        $(seachEl).children('input').bind('keyup', function (e) {
                        var value = $(this).val();
        if (value) {
            $(customSelect).addClass('searchActive');
        $(seachElOption).each(function () {
                                if ($(this).text().search(new RegExp(value, "i")) < 0) {
            // not item
            $(this).fadeOut();
                                } else {
            // have item
            $(this).fadeIn();
                                }
                            })
                        } else {
            $(customSelect).removeClass('searchActive');
        $(seachElOption).fadeIn();
                        }
                    })

                });
            };
        $(document).click(function () {
                if (elActive != '') {
            $(elActive).removeClass('onCF');
        $(elActive).removeClass('searchActive');
                }
            })
        }(jQuery));

        $(function () {
            var event_change = $('#event-change');
        $(".select").selectCF({
            change: function () {
            let answerId = $(this).val();
        let id = $(this).attr('id');
        if (answerId == 0) {
            $("#nxt_" + id).attr("hidden", true);
        return false;
                    }
        dataList = { };
        dataList.questionId = id;
        dataList.answerId = answerId;
        $.each(questionaire, function (i, el) {
                        if (this.questionId == id) {
            questionaire.splice(i, 1);
                        }
                    });
        questionaire.push(dataList);
        $("#nxt_" + id).attr("hidden", false);

                    //var value = $(this).val();
                    //var text = $(this).children('option:selected').html();
                    //event_change.html(value + ' : ' + text);
                }
            });
        $(".test").selectCF({
            color: "rgb(112, 112, 112)",
        backgroundColor: "#663052",
            });
        })


        //var questionaire = [];

        $(document).ready(function () {
            var questionId = $("#firstQuestion").text();
        var id = "@ViewBag.Check";
        var QA="@ViewBag.QAresult";
        if (id == "update") {
            $('.preloader').css('display', 'block');
        $.ajax({
            //dataType: 'json',
            type: "POST",
        url: "/Questionnaire/GetUserQuestionaire",
        success: function (response) {
                        var data = response.questionModel;
        if (data) {
                            for (var i = 0; i < data.length; i++) {
                                debugger
        if (data[i].questionId == 1 || data[i].questionId == 24 || data[i].questionId == 30 || data[i].questionId == 33 || data[i].questionId == 32 || data[i].questionId == 7 || data[i].questionId == 6 || data[i].questionId == 9 || data[i].questionId == 10) {
                                    if (data[i].questionId == 30) {
                                        if (data[i].answerList[0].answerId == 112) {
            $("#txt_30").css("display", "block");
        $("#txt_30").val(data[i].answerList[0].answer);
                                        }

                                    }
        if ($('#isDigitalAnalysis').val() == "1") {
                                        if (data[i].questionId == 1) {
            $("#" + data[i].answerList[0].answerId).attr('checked', 'checked');
        $("#nxt_3").attr("hidden", false);

                                        }
                                    }

        $("#" + data[i].answerList[0].answerId).attr('checked', 'checked');
        $("#nxt_" + data[i].questionId).attr("hidden", false);
        dataList = { };
        dataList.questionId = data[i].questionId;
        dataList.answerId = "" + data[i].answerList[0].answerId + "";
        questionaire.push(dataList);
                                }

        if (data[i].questionId == 2 || data[i].questionId == 3 || data[i].questionId == 4 || data[i].questionId == 5) {
                                    for (var j = 0; j < data[i].answerList.length; j++) {
            $("#radio" + data[i].answerList[j].answerId).attr('checked', 'checked');
        dataList = { };
        dataList.questionId = data[i].questionId;
        dataList.answerId = "" + data[i].answerList[j].answerId + "";
        questionaire.push(dataList);
                                    }
        $("#nxt_" + data[i].questionId).attr("hidden", false);
                                }
        if (data[i].questionId == 18) {
                                    for (var j = 0; j < data[i].answerList.length; j++) {
            $('input[value ="' + data[i].answerList[j].answerId + '"]').attr('checked', 'checked');
        dataList = { };
        dataList.questionId = data[i].questionId;
        dataList.answerId = "" + data[i].answerList[j].answerId + "";
        questionaire.push(dataList);
                                    }
        $("#nxt_" + data[i].questionId).attr("hidden", false);
                                }

        if (data[i].questionId == 8 || data[i].questionId == 17) {
            $('input[value ="' + data[i].answerList[0].answerId + '"]').attr('checked', 'checked');
        $("#nxt_" + data[i].questionId).attr("hidden", false);
        dataList = { };
        dataList.questionId = data[i].questionId;
        dataList.answerId = "" + data[i].answerList[0].answerId + "";
        questionaire.push(dataList);
                                }
                                //if (data[i].questionId == 6 || data[i].questionId == 9 || data[i].questionId == 10 || data[i].questionId == 33 || data[i].questionId == 32
                                //    || data[i].questionId == 7) {
                                //    var answerText = null;
                                //    $('li').each(function () {
                                //        if ($(this).attr('value') == 0) {
                                //            $(this).removeClass('selected');
                                //        }
                                //        if ($(this).attr('value') == data[i].answerList[0].answerId) {
                                //            $(this).addClass('selected');
                                //            answerText = $(this).text();
                                //            dataList = { };
                                //            dataList.questionId = data[i].questionId;
                                //            dataList.answerId = "" + data[i].answerList[0].answerId + "";
                                //            questionaire.push(dataList);
                                //        }
                                //    });
                                //    $('.select' + data[i].questionId + ' .titleCF').text(answerText);
                                //    $("#nxt_" + data[i].questionId).attr("hidden", false);

                                //    // $("li").find("option[value=" + data[i].answerList[0].answerId + "]").addClass('selected');
                                //}
                                if (data[i].questionId == 11 || data[i].questionId == 14 || data[i].questionId == 15 || data[i].questionId == 16
        || data[i].questionId == 28) {
                                    var valArray = [];
        for (var j = 0; j < data[i].answerList.length; j++) {
                                        var optionVal = data[i].answerList[j].answerId;
        valArray.push(optionVal);
        var selText = $("#" + data[i].questionId).find("option[value=" + optionVal + "]").text();
        var describe = "select2-" + data[i].questionId + "-container-choice-vqv2-" + data[i].answerList[j].answerId;
        $('#select2-' + data[i].questionId + '-container').append('<li data-select2-id="select2-data-' + data[i].answerList[j].answerId + '-o883" class="select2-selection__choice" title="' + selText + '"><button type="button" onclick="removeLi(this, ' + data[i].answerList[j].answerId + ')" class="select2-selection__choice__remove" tabindex="-1" title="Remove item" aria-describedby=' + describe + ' aria-label="Remove item"><span aria-hidden="true" id=' + describe + '>×</span></button><span class="select2-selection__choice__display">' + selText + '</span></li>');
        dataList = { };
        dataList.questionId = data[i].questionId;
        dataList.answerId = "" + data[i].answerList[j].answerId + "";
        questionaire.push(dataList);
                                    }
        $('#' + data[i].questionId).val(valArray);
        $("#nxt_" + data[i].questionId).attr("hidden", false);
                                }
        if (data[i].questionId == 25 || data[i].questionId == 31 || data[i].questionId == 19 || data[i].questionId == 37) {
                                    var valArray = [];
        var questionId;
        for (var j = 0; j < data[i].answerList.length; j++) {
                                        var optionVal = data[i].answerList[j].answerId;
        valArray.push(optionVal);
        var selText = $("#drp" + data[i].questionId).find("option[value=" + optionVal + "]").text();
        var describe = "select2-drp" + data[i].questionId + "-container-choice-vqv2-" + data[i].answerList[j].answerId;
        $('#select2-drp' + data[i].questionId + '-container').append('<li data-select2-id="select2-data-' + data[i].answerList[j].answerId + '-o883" class="select2-selection__choice" title="' + selText + '"><button type="button" onclick="removeLi(this, ' + data[i].answerList[j].answerId + ')" class="select2-selection__choice__remove" tabindex="-1" title="Remove item" aria-describedby=' + describe + ' aria-label="Remove item"><span aria-hidden="true" id=' + describe + '>×</span></button><span class="select2-selection__choice__display">' + selText + '</span></li>');
        dataList = { };
        dataList.questionId = data[i].questionId;
        dataList.answerId = "" + data[i].answerList[j].answerId + "";
        questionaire.push(dataList);
                                    }
        $('#drp' + data[i].questionId).val(valArray);
        $("#nxt_" + data[i].questionId).attr("hidden", false);

        $('.maxThree').select2({
            placeholder: "-- Select --",
        closeOnSelect: false,
        maximumSelectionLength: 3
                                    }).on("select2:closing", function (e) {
            // e.preventDefault();
        }).on("select2:closed", function (e) {
            // list.select2("open");
        }).on('select2:opening', function (e) {
            $(window).scrollTop(0);
        var scrollTop = $(window).scrollTop();
        var y = $(window).scrollTop();
                                        if (scrollTop + $(window).innerHeight() >= this.scrollHeight) {
            $(window).scrollTop(y + 1);
                                        } else if (scrollTop <= 0) {
            $(window).scrollTop(y - 1);
                                        }
                                    })
                                }

        if (data[i].questionId == 12 || data[i].questionId == 13
        || data[i].questionId == 20 || data[i].questionId == 35 || data[i].questionId == 36 || data[i].questionId == 21 || data[i].questionId == 23 || data[i].questionId == 26 || data[i].questionId == 27 || data[i].questionId == 38) {
            $('#txt_' + data[i].questionId).val(data[i].answerList[0].answer);
        $("#nxt_" + data[i].questionId).attr("hidden", false);
                                    //dataList = { };
                                    //dataList.questionId = data[i].questionId;
                                    //dataList.descriptiveAnswer = "" + data[i].answerList[0].answer + "";
                                    //questionaire.push(dataList);
                                }

        if (data[i].questionId == 29) {
                                    for (var j = 0; j < data[i].answerList.length; j++) {
                                        if (data[i].answerList[j].answer.includes('facebook'))
        $('#txt_29').val(data[i].answerList[j].answer);
        if (data[i].answerList[j].answer.includes('twitter'))
        $('#txt_30').val(data[i].answerList[j].answer);
        if (data[i].answerList[j].answer.includes('instagram'))
        $('#txt_31').val(data[i].answerList[j].answer);
        if (data[i].answerList[j].answer.includes('youtube'))
        $('#txt_32').val(data[i].answerList[j].answer);
                                    }
        $("#nxt_" + data[i].questionId).attr("hidden", false);
                                }

        if (data[i].questionId == 22) {
            $('#imageShowing').attr('src', data[i].answerList[0].answer)
                                    $('#uploadedImage').removeAttr('hidden');
        $('#dragHere').attr("hidden", true);
        $('#hairImage').val('');
        filename = (data[i].answerList[0].answer).substring((data[i].answerList[0].answer).lastIndexOf('/') + 1);
        $("#nxt_22").attr("hidden", false);
                                }
                            }
                        }

        $('.preloader').css('display', 'none');
                    },
                });
            }
        });

        function removeLi(event, answerId) {
            $(event).parent().remove();
        $.each(questionaire, function (i, el) {
                if (this.answerId == answerId) {
            questionaire.splice(i, 1);
                }
            });
           // var ele = $(this).attr('aria-describedby');/Questionnaire/SaveImage
        }

        function getUrlVars() {
            var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
            }
        return vars;
        }
        function redirectDigAss() {
            window.location.href = "/Questionnaire/DigitalAssessment";
        }
        function redirectHairProfile() {
            window.location.href = "/HairProfile/CustomerHair";
        }

