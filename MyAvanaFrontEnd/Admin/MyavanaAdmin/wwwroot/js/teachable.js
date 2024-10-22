
// More API functions here:
// https://github.com/googlecreativelab/teachablemachine-community/tree/master/libraries/pose

// the link to your model provided by Teachable Machine export panel

// const URL = "https://teachablemachine.withgoogle.com/models/kijLW5wFL/";
const URL = "https://teachablemachine.withgoogle.com/models/Anc4kDudS/";
let model, webcam, ctx, labelContainer, maxPredictions;

async function init() {
    const modelURL = URL + "model.json";
    const metadataURL = URL + "metadata.json";

    // load the model and metadata
    // Refer to tmImage.loadFromFiles() in the API to support files from a file picker
    // Note: the pose library adds a tmPose object to your window (window.tmPose)
    model = await tmPose.load(modelURL, metadataURL);
    maxPredictions = model.getTotalClasses();

    document.getElementById("instruct").classList.remove("hidden");
    document.getElementById("temp_img").classList.add("hidden");

    // Convenience function to setup a webcam
    const size = 300;
    const flip = false; // whether to flip the webcam
    webcam = new tmPose.Webcam(300, 300, flip); // width, height, flip
    await webcam.setup({ facingMode: "user" }); // request access to the webcam

    webcam.webcam.addEventListener("loadeddata", () => {
        webcam.play();
        window.requestAnimationFrame(loop);
    });
    // await webcam.play();

    document.getElementById("cnvs-parent").appendChild(webcam.webcam); // webcam object needs to be added in any case to make this work on iOS
    // grab video-object in any way you want and set the attributes --> **"muted" and "playsinline"**
    //var wc = document.getElementsByTagName("video")[0]; // written with "setAttribute" bc. iOS buggs otherwise :-)
    var wc = document.getElementById("cnvs-parent").querySelectorAll("video")[0]; // written with "setAttribute" bc. iOS buggs otherwise :-)
    wc.setAttribute("playsinline", true);
    wc.setAttribute("muted", true);
    wc.setAttribute("autoplay", true);
    wc.style.visibility = "hidden";
    wc.style.position = "absolute";
    wc.id = "videoElement";
    wc.setAttribute("autoplay", true);

    wc.addEventListener("loadeddata", () => { });

    // const player = document.getElementById('videoElement');
    // const context = document.getElementById('canvas').getContext('2d');
    // context.drawImage(player, 0, 0, 300, 300);

    //document.getElementById('webcam-container').appendChild(webcam.webcam)

    // append/get elements to the DOM
    const canvas = document.getElementById("canvas");
    canvas.parentElement.className = "canvas-container";
    canvas.parentElement.style.display = "flex";
    canvas.parentElement.style.gap = "1em";
    canvas.parentElement.style.justifyContent = "center";
    canvas.parentElement.style.margin = "1em";
    canvas.width = size;
    canvas.height = size;

    ctx = canvas.getContext("2d");

    labelContainer = document.getElementById("label-container");
    for (let i = 0; i < maxPredictions; i++) {
        // and class labels
        var cont = document.createElement("div");
        labelContainer.appendChild(cont);
    }


    //await webcam.play();
    // window.requestAnimationFrame(loop);
}

function initAgain() {


    //hide the retake button and the continue button
    document.getElementById("retake-btn").classList.add("hidden");
    // document.getElementById("continue-btn").classList.add("hidden");
    document.getElementById("newStart-btn").classList.remove("hidden");
    // document.getElementById("canvas").classList.add("hidden");

    //hide img attribute from id cnvs-parent
    //var parentCanvas = document.getElementById("cnvs-parent");
    //parentCanvas.querySelectorAll("img").forEach((n) => n.remove());
    //parentCanvas.querySelectorAll("video").forEach((n) => n.remove());

    var still = document.getElementById("canvas");
    still.style.margin = "0 auto";
    still.style.display = "flex";
    //camCounter();
    webcam.update();
    init();
}

async function loop(timestamp) {
    webcam.update(); // update the webcam frame
    await predict();
    window.requestAnimationFrame(loop);
}

var predState = 0;
var predSides = 0;

var counterState = 0;

async function predict() {
    // Prediction #1: run input through posenet
    // estimatePose can take in an image, video or canvas html element
    let wc2 = document.getElementById("videoElement");
    wc2.setAttribute("width", 300);
    wc2.setAttribute("height", 300);
    wc2.style.visibility = "hidden";
    wc2.style.position = "absolute";

    const { pose, posenetOutput } = await model.estimatePose(webcam.webcam);
    // Prediction 2: run input through teachable machine classification model
    const prediction = await model.predict(posenetOutput);

    for (let i = 0; i < maxPredictions; i++) {
        //showing debug labels
        // const classPrediction = prediction[i].className + ": " + prediction[i].probability.toFixed(2);
        // labelContainer.childNodes[i].innerHTML = classPrediction;

        //console.log(prediction[1].probability.toFixed(2));
        predState = prediction[1].probability.toFixed(2);

        if (prediction[4].probability.toFixed(2) > 0.75 || prediction[5].probability.toFixed(2) > 0.75) {
            predSides = 1;
        } else {
            predSides = 0;
        }
        instructUser(prediction[0].probability.toFixed(1), prediction[1].probability.toFixed(2), prediction[2].probability.toFixed(2), prediction[3].probability.toFixed(2), prediction[4].probability.toFixed(2), prediction[5].probability.toFixed(2));
    }
    drawPose(pose);
}

function instructUser(FaceFront, TopHead, TooFar, TooClose, FacingL, FacingR) {
    if (FaceFront > 0.75) {
        document.getElementById("instruct").innerHTML = "Face Aligned properly; Please tilt your head down or to the side.";
    } else if (TopHead > 0.75) {
        document.getElementById("instruct").innerHTML = "Capturing.. Please stay still.";
    } else if (TooFar > 0.75) {
        document.getElementById("instruct").innerHTML = "Too Far";
    } else if (TooClose > 0.75) {
        document.getElementById("instruct").innerHTML = "Too Close";
    } else if (FacingL > 0.75) {
        document.getElementById("instruct").innerHTML = "Capturing the right side of your hair.";
    } else if (FacingR > 0.75) {
        document.getElementById("instruct").innerHTML = "Capturing the left side of your hair.";
    } else {
        document.getElementById("instruct").innerHTML = "";
    }
}

camCounter();

function beep() {
    var snd = new Audio(
        "data:audio/wav;base64,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qJrx4KOQjahZxWbcZgztj2c49nKmkId44S71j0c8eV9yDK6uPRzx5X18eDvjvQ6yKo9ZSS6l//8elePK/Lf//IInrOF/FvDoADYAGBMGb7FtErm5MXMlmPAJQVgWta7Zx2go+8xJ0UiCb8LHHdftWyLJE0QIAIsI+UbXu67dZMjmgDGCGl1H+vpF4NSDckSIkk7Vd+sxEhBQMRU8j/12UIRhzSaUdQ+rQU5kGeFxm+hb1oh6pWWmv3uvmReDl0UnvtapVaIzo1jZbf/pD6ElLqSX+rUmOQNpJFa/r+sa4e/pBlAABoAAAAA3CUgShLdGIxsY7AUABPRrgCABdDuQ5GC7DqPQCgbbJUAoRSUj+NIEig0YfyWUho1VBBBA//uQZB4ABZx5zfMakeAAAAmwAAAAF5F3P0w9GtAAACfAAAAAwLhMDmAYWMgVEG1U0FIGCBgXBXAtfMH10000EEEEEECUBYln03TTTdNBDZopopYvrTTdNa325mImNg3TTPV9q3pmY0xoO6bv3r00y+IDGid/9aaaZTGMuj9mpu9Mpio1dXrr5HERTZSmqU36A3CumzN/9Robv/Xx4v9ijkSRSNLQhAWumap82WRSBUqXStV/YcS+XVLnSS+WLDroqArFkMEsAS+eWmrUzrO0oEmE40RlMZ5+ODIkAyKAGUwZ3mVKmcamcJnMW26MRPgUw6j+LkhyHGVGYjSUUKNpuJUQoOIAyDvEyG8S5yfK6dhZc0Tx1KI/gviKL6qvvFs1+bWtaz58uUNnryq6kt5RzOCkPWlVqVX2a/EEBUdU1KrXLf40GoiiFXK///qpoiDXrOgqDR38JB0bw7SoL+ZB9o1RCkQjQ2CBYZKd/+VJxZRRZlqSkKiws0WFxUyCwsKiMy7hUVFhIaCrNQsKkTIsLivwKKigsj8XYlwt/WKi2N4d//uQRCSAAjURNIHpMZBGYiaQPSYyAAABLAAAAAAAACWAAAAApUF/Mg+0aohSIRobBAsMlO//Kk4soosy1JSFRYWaLC4qZBYWFRGZdwqKiwkNBVmoWFSJkWFxX4FFRQWR+LsS4W/rFRb/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////VEFHAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAU291bmRib3kuZGUAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAMjAwNGh0dHA6Ly93d3cuc291bmRib3kuZGUAAAAAAAAAACU="
    );
    snd.play();
}

function captureAudio() {
    var snd = new Audio("https://www.zapsplat.com/wp-content/uploads/2015/sound-effects-one/technology_camera_shutter_release_nikon_f4_001.mp3");
    snd.play();
}

function camCounter() {
    var count = setInterval(() => {
        //counts here!
        if ((predState == 1 && counterState < 3) || (predSides == 1 && counterState < 3)) {
            counterState++;
            console.log("Counting!", counterState);
            beep();

            //captures here!
        } else if ((predState == 1 && counterState == 3) || (predSides == 1 && counterState == 3)) {
            document.getElementById("instruct").classList.add("hidden");
            //show the retake button and the continue button
            document.getElementById("retake-btn").classList.remove("hidden");
            //document.getElementById("continue-btn").classList.remove("hidden");
            document.getElementById("newStart-btn").classList.add("hidden");
            document.getElementById("canvas").classList.add("hidden");
            //document.getElementById("retake").classList.add("visible");
            counterState = 0;
            captureAudio();
            clearInterval(count);
            var still = document.getElementById("canvas");

            //post to the next screen here
            var stillData = still.toDataURL("image/png");
            postImageToAnalyze(stillData);
            //still.style.display = "none";
            Image = document.createElement("img");
            Image.style.borderRadius = "25px";
            Image.style.transform = "scaleX(-1)";
            Image.src = still.toDataURL("image/png");

            still.after(Image);

            //resets here!
        } else if (predState < 0.9 || predSides < 0.9) {
            counterState = 0;
        }
    }, 1000);
}

function DataURIToBlob(dataURI) {
    console.log;
    const splitDataURI = dataURI.split(",");
    const byteString = splitDataURI[0].indexOf("base64") >= 0 ? atob(splitDataURI[1]) : decodeURI(splitDataURI[1]);
    const mimeString = splitDataURI[0].split(":")[1].split(";")[0];
    const ia = new Uint8Array(byteString.length);
    for (let i = 0; i < byteString.length; i++) ia[i] = byteString.charCodeAt(i);
    return new Blob([ia], { type: mimeString });
}

function postImageToAnalyze(imgData, PaymentId) {
    const API_URL = "https://analysisai01.test.com";
    const formData = new FormData();
    formData.append("imageFile", DataURIToBlob(imgData));
    formData.append("Content-Type", "multipart/form-data");
    var userid = GetQuerystringValue('userId');
    fetch(API_URL + "/api/v2/Image/classifyimage", {
        method: "post",
        headers: {
            "x-api-key": "1ebf78f8-e63a-4753-879d-f0d9bd7701a7",
        },
        body: formData,
    })
        .then((response) => response.json())
        .then((data) => {
            //resultsPush(data.hairTypeLabel, data.hairTextureLabel, data.label);
            DigitalAssessmentModel = {
                AIResult: JSON.stringify(data),
                HairType: data.hairTypeLabel,
                Userid: userid,
                ImageData: imgData.replace('data:', '')
                    .replace(/^.+,/, ''),
                PaymentId: PaymentId
            }
            $.ajax({
                type: "POST",
                url: "/Questionnaire/SaveCustomerAIResultandImage",
                data: DigitalAssessmentModel,
                success: function (response) {
                    window.open("/HairAIResult/HairAIResult?id=" + userid, "_self")
                    if (response) {
                      /*  window.location.href = "/Questionnaire/SaveDigitalAssessment";*/
                    }
                    else {
                        $('#failureMessage').text("Something went wrong. Please try again later.");
                        $('.alert-danger').css("display", "block");
                        $('.alert-danger').delay(2000).fadeOut();
                    }
                },
                failure: function (response) {
                },
                error: function (response) {
                },
            });
        });
}

function drawPose(pose) {
    if (webcam.canvas) {
        ctx.drawImage(webcam.canvas, 0, 0);
        // draw the keypoints and skeleton

        if (pose) {
            // const minPartConfidence = 0.5;
            // tmPose.drawKeypoints(pose.keypoints, minPartConfidence, ctx);
            // tmPose.drawSkeleton(pose.keypoints, minPartConfidence, ctx);

            var ctx2 = document.getElementById("canvas").getContext("2d");
            var ctx2_styler = document.getElementById("canvas");
            ctx2.strokeStyle = "grey";
            ctx2.lineWidth = 20;
            ctx2.fillStyle = "grey";
            ctx2_styler.style.borderRadius = "25px";
            ctx2_styler.overflow = "hidden";
            ctx2.stroke();
            ctx2.beginPath();
            // ctx2.moveTo(pose.keypoints[0].position.x, pose.keypoints[0].position.y);
            ctx2.lineTo(pose.keypoints[1].position.x + 15, pose.keypoints[1].position.y - 10);
            ctx2.lineTo(pose.keypoints[2].position.x - 15, pose.keypoints[2].position.y - 10);
            ctx2.lineTo(pose.keypoints[2].position.x - 15, pose.keypoints[2].position.y + 40);
            ctx2.lineTo(pose.keypoints[1].position.x + 15, pose.keypoints[1].position.y + 40);
            ctx2.closePath();
            ctx2.fill();
        }
    }
}

//if numberX stays at one for 10 seconds, then it will trigger the function
function tickerTrigger(numberX) {
    var ticker = 0;
    var tickerInterval = setInterval(function () {
        numberX == 1 ? ticker++ : (ticker = 0);
        if (ticker == 3) {
            clearInterval(tickerInterval);
            console.log("triggered");
            //do something here
        }
    }, 1000);
}
function GetQuerystringValue(param) {
    var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < url.length; i++) {
        var urlparam = url[i].split('=');
        if (urlparam[0] == param) {
            return urlparam[1];
        }
    }
}