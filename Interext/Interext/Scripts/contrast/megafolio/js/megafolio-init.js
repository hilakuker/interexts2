/*!
 * jQuery Transit - CSS3 transitions and transformations
 * Copyright(c) 2011 Rico Sta. Cruz <rico@ricostacruz.com>
 * MIT Licensed.
 *
 * http://ricostacruz.com/jquery.transit
 * http://github.com/rstacruz/jquery.transit
 */

/*!
jQuery WaitForImages

Copyright (c) 2012 Alex Dickson

Permission is hereby granted, free of charge, to any person
obtaining a copy of this software and associated documentation
files (the "Software"), to deal in the Software without
restriction, including without limitation the rights to use,
copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the
Software is furnished to do so, subject to the following
conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.


https://github.com/alexanderdickson/waitForImages


 */

// WAIT FOR IMAGES
/*
 * waitForImages 1.4
 * -----------------
 * Provides a callback when all images have loaded in your given selector.
 * http://www.alexanderdickson.com/
 *
 *
 * Copyright (c) 2011 Alex Dickson
 * Licensed under the MIT licenses.
 * See website for more info.
 *
 */

// EASINGS

(function(e){function t(e){if(e in u.style)return e;var t=["Moz","Webkit","O","ms"],n=e.charAt(0).toUpperCase()+e.substr(1);if(e in u.style)return e;for(e=0;e<t.length;++e){var r=t[e]+n;if(r in u.style)return r}}function n(e){"string"===typeof e&&this.parse(e);return this}function r(t,n,r,i){var s=[];e.each(t,function(t){t=e.camelCase(t);t=e.transit.propertyMap[t]||e.cssProps[t]||t;t=t.replace(/([A-Z])/g,function(e){return"-"+e.toLowerCase()});-1===e.inArray(t,s)&&s.push(t)});e.cssEase[r]&&(r=e.cssEase[r]);var u=""+o(n)+" "+r;0<parseInt(i,10)&&(u+=" "+o(i));var a=[];e.each(s,function(e,t){a.push(t+" "+u)});return a.join(", ")}function i(t,n){n||(e.cssNumber[t]=!0);e.transit.propertyMap[t]=a.transform;e.cssHooks[t]={get:function(n){return e(n).css("transit:transform").get(t)},set:function(n,r){var i=e(n).css("transit:transform");i.setFromString(t,r);e(n).css({"transit:transform":i})}}}function s(e,t){return"string"===typeof e&&!e.match(/^[\-0-9\.]+$/)?e:""+e+t}function o(t){e.fx.speeds[t]&&(t=e.fx.speeds[t]);return s(t,"ms")}e.transit={version:"0.9.9",propertyMap:{marginLeft:"margin",marginRight:"margin",marginBottom:"margin",marginTop:"margin",paddingLeft:"padding",paddingRight:"padding",paddingBottom:"padding",paddingTop:"padding"},enabled:!0,useTransitionEnd:!1};var u=document.createElement("div"),a={},f=-1<navigator.userAgent.toLowerCase().indexOf("chrome");a.transition=t("transition");a.transitionDelay=t("transitionDelay");a.transform=t("transform");a.transformOrigin=t("transformOrigin");u.style[a.transform]="";u.style[a.transform]="rotateY(90deg)";a.transform3d=""!==u.style[a.transform];var l=a.transitionEnd={transition:"transitionEnd",MozTransition:"transitionend",OTransition:"oTransitionEnd",WebkitTransition:"webkitTransitionEnd",msTransition:"MSTransitionEnd"}[a.transition]||null,c;for(c in a)a.hasOwnProperty(c)&&"undefined"===typeof e.support[c]&&(e.support[c]=a[c]);u=null;e.cssEase={_default:"ease","in":"ease-in",out:"ease-out","in-out":"ease-in-out",snap:"cubic-bezier(0,1,.5,1)",easeOutCubic:"cubic-bezier(.215,.61,.355,1)",easeInOutCubic:"cubic-bezier(.645,.045,.355,1)",easeInCirc:"cubic-bezier(.6,.04,.98,.335)",easeOutCirc:"cubic-bezier(.075,.82,.165,1)",easeInOutCirc:"cubic-bezier(.785,.135,.15,.86)",easeInExpo:"cubic-bezier(.95,.05,.795,.035)",easeOutExpo:"cubic-bezier(.19,1,.22,1)",easeInOutExpo:"cubic-bezier(1,0,0,1)",easeInQuad:"cubic-bezier(.55,.085,.68,.53)",easeOutQuad:"cubic-bezier(.25,.46,.45,.94)",easeInOutQuad:"cubic-bezier(.455,.03,.515,.955)",easeInQuart:"cubic-bezier(.895,.03,.685,.22)",easeOutQuart:"cubic-bezier(.165,.84,.44,1)",easeInOutQuart:"cubic-bezier(.77,0,.175,1)",easeInQuint:"cubic-bezier(.755,.05,.855,.06)",easeOutQuint:"cubic-bezier(.23,1,.32,1)",easeInOutQuint:"cubic-bezier(.86,0,.07,1)",easeInSine:"cubic-bezier(.47,0,.745,.715)",easeOutSine:"cubic-bezier(.39,.575,.565,1)",easeInOutSine:"cubic-bezier(.445,.05,.55,.95)",easeInBack:"cubic-bezier(.6,-.28,.735,.045)",easeOutBack:"cubic-bezier(.175, .885,.32,1.275)",easeInOutBack:"cubic-bezier(.68,-.55,.265,1.55)"};e.cssHooks["transit:transform"]={get:function(t){return e(t).data("transform")||new n},set:function(t,r){var i=r;i instanceof n||(i=new n(i));t.style[a.transform]="WebkitTransform"===a.transform&&!f?i.toString(!0):i.toString();e(t).data("transform",i)}};e.cssHooks.transform={set:e.cssHooks["transit:transform"].set};"1.8">e.fn.jquery&&(e.cssHooks.transformOrigin={get:function(e){return e.style[a.transformOrigin]},set:function(e,t){e.style[a.transformOrigin]=t}},e.cssHooks.transition={get:function(e){return e.style[a.transition]},set:function(e,t){e.style[a.transition]=t}});i("scale");i("translate");i("rotate");i("rotateX");i("rotateY");i("rotate3d");i("perspective");i("skewX");i("skewY");i("x",!0);i("y",!0);n.prototype={setFromString:function(e,t){var r="string"===typeof t?t.split(","):t.constructor===Array?t:[t];r.unshift(e);n.prototype.set.apply(this,r)},set:function(e){var t=Array.prototype.slice.apply(arguments,[1]);this.setter[e]?this.setter[e].apply(this,t):this[e]=t.join(",")},get:function(e){return this.getter[e]?this.getter[e].apply(this):this[e]||0},setter:{rotate:function(e){this.rotate=s(e,"deg")},rotateX:function(e){this.rotateX=s(e,"deg")},rotateY:function(e){this.rotateY=s(e,"deg")},scale:function(e,t){void 0===t&&(t=e);this.scale=e+","+t},skewX:function(e){this.skewX=s(e,"deg")},skewY:function(e){this.skewY=s(e,"deg")},perspective:function(e){this.perspective=s(e,"px")},x:function(e){this.set("translate",e,null)},y:function(e){this.set("translate",null,e)},translate:function(e,t){void 0===this._translateX&&(this._translateX=0);void 0===this._translateY&&(this._translateY=0);null!==e&&void 0!==e&&(this._translateX=s(e,"px"));null!==t&&void 0!==t&&(this._translateY=s(t,"px"));this.translate=this._translateX+","+this._translateY}},getter:{x:function(){return this._translateX||0},y:function(){return this._translateY||0},scale:function(){var e=(this.scale||"1,1").split(",");e[0]&&(e[0]=parseFloat(e[0]));e[1]&&(e[1]=parseFloat(e[1]));return e[0]===e[1]?e[0]:e},rotate3d:function(){for(var e=(this.rotate3d||"0,0,0,0deg").split(","),t=0;3>=t;++t)e[t]&&(e[t]=parseFloat(e[t]));e[3]&&(e[3]=s(e[3],"deg"));return e}},parse:function(e){var t=this;e.replace(/([a-zA-Z0-9]+)\((.*?)\)/g,function(e,n,r){t.setFromString(n,r)})},toString:function(e){var t=[],n;for(n in this)if(this.hasOwnProperty(n)&&(a.transform3d||!("rotateX"===n||"rotateY"===n||"perspective"===n||"transformOrigin"===n)))"_"!==n[0]&&(e&&"scale"===n?t.push(n+"3d("+this[n]+",1)"):e&&"translate"===n?t.push(n+"3d("+this[n]+",0)"):t.push(n+"("+this[n]+")"));return t.join(" ")}};e.fn.transition=e.fn.transit=function(t,n,i,s){var u=this,f=0,c=!0;"function"===typeof n&&(s=n,n=void 0);"function"===typeof i&&(s=i,i=void 0);"undefined"!==typeof t.easing&&(i=t.easing,delete t.easing);"undefined"!==typeof t.duration&&(n=t.duration,delete t.duration);"undefined"!==typeof t.complete&&(s=t.complete,delete t.complete);"undefined"!==typeof t.queue&&(c=t.queue,delete t.queue);"undefined"!==typeof t.delay&&(f=t.delay,delete t.delay);"undefined"===typeof n&&(n=e.fx.speeds._default);"undefined"===typeof i&&(i=e.cssEase._default);n=o(n);var h=r(t,n,i,f),v=e.transit.enabled&&a.transition?parseInt(n,10)+parseInt(f,10):0;if(0===v)return n=c,i=function(e){u.css(t);s&&s.apply(u);e&&e()},!0===n?u.queue(i):n?u.queue(n,i):i(),u;var m={};n=c;i=function(n){var r=0;"MozTransition"===a.transition&&25>r&&(r=25);window.setTimeout(function(){var r=!1,i=function(){r&&u.unbind(l,i);0<v&&u.each(function(){this.style[a.transition]=m[this]||null});"function"===typeof s&&s.apply(u);"function"===typeof n&&n()};0<v&&l&&e.transit.useTransitionEnd?(r=!0,u.bind(l,i)):window.setTimeout(i,v);u.each(function(){0<v&&(this.style[a.transition]=h);e(this).css(t)})},r)};!0===n?u.queue(i):n?u.queue(n,i):i();return this};e.transit.getTransitionValue=r})(jQuery);(function(e,t){jQuery.easing["jswing"]=jQuery.easing["swing"];jQuery.extend(jQuery.easing,{def:"easeOutQuad",swing:function(e,t,n,r,i){return jQuery.easing[jQuery.easing.def](e,t,n,r,i)},easeInQuad:function(e,t,n,r,i){return r*(t/=i)*t+n},easeOutQuad:function(e,t,n,r,i){return-r*(t/=i)*(t-2)+n},easeInOutQuad:function(e,t,n,r,i){if((t/=i/2)<1)return r/2*t*t+n;return-r/2*(--t*(t-2)-1)+n},easeInCubic:function(e,t,n,r,i){return r*(t/=i)*t*t+n},easeOutCubic:function(e,t,n,r,i){return r*((t=t/i-1)*t*t+1)+n},easeInOutCubic:function(e,t,n,r,i){if((t/=i/2)<1)return r/2*t*t*t+n;return r/2*((t-=2)*t*t+2)+n},easeInQuart:function(e,t,n,r,i){return r*(t/=i)*t*t*t+n},easeOutQuart:function(e,t,n,r,i){return-r*((t=t/i-1)*t*t*t-1)+n},easeInOutQuart:function(e,t,n,r,i){if((t/=i/2)<1)return r/2*t*t*t*t+n;return-r/2*((t-=2)*t*t*t-2)+n},easeInQuint:function(e,t,n,r,i){return r*(t/=i)*t*t*t*t+n},easeOutQuint:function(e,t,n,r,i){return r*((t=t/i-1)*t*t*t*t+1)+n},easeInOutQuint:function(e,t,n,r,i){if((t/=i/2)<1)return r/2*t*t*t*t*t+n;return r/2*((t-=2)*t*t*t*t+2)+n},easeInSine:function(e,t,n,r,i){return-r*Math.cos(t/i*(Math.PI/2))+r+n},easeOutSine:function(e,t,n,r,i){return r*Math.sin(t/i*(Math.PI/2))+n},easeInOutSine:function(e,t,n,r,i){return-r/2*(Math.cos(Math.PI*t/i)-1)+n},easeInExpo:function(e,t,n,r,i){return t==0?n:r*Math.pow(2,10*(t/i-1))+n},easeOutExpo:function(e,t,n,r,i){return t==i?n+r:r*(-Math.pow(2,-10*t/i)+1)+n},easeInOutExpo:function(e,t,n,r,i){if(t==0)return n;if(t==i)return n+r;if((t/=i/2)<1)return r/2*Math.pow(2,10*(t-1))+n;return r/2*(-Math.pow(2,-10*--t)+2)+n},easeInCirc:function(e,t,n,r,i){return-r*(Math.sqrt(1-(t/=i)*t)-1)+n},easeOutCirc:function(e,t,n,r,i){return r*Math.sqrt(1-(t=t/i-1)*t)+n},easeInOutCirc:function(e,t,n,r,i){if((t/=i/2)<1)return-r/2*(Math.sqrt(1-t*t)-1)+n;return r/2*(Math.sqrt(1-(t-=2)*t)+1)+n},easeInElastic:function(e,t,n,r,i){var s=1.70158;var o=0;var u=r;if(t==0)return n;if((t/=i)==1)return n+r;if(!o)o=i*.3;if(u<Math.abs(r)){u=r;var s=o/4}else var s=o/(2*Math.PI)*Math.asin(r/u);return-(u*Math.pow(2,10*(t-=1))*Math.sin((t*i-s)*2*Math.PI/o))+n},easeOutElastic:function(e,t,n,r,i){var s=1.70158;var o=0;var u=r;if(t==0)return n;if((t/=i)==1)return n+r;if(!o)o=i*.3;if(u<Math.abs(r)){u=r;var s=o/4}else var s=o/(2*Math.PI)*Math.asin(r/u);return u*Math.pow(2,-10*t)*Math.sin((t*i-s)*2*Math.PI/o)+r+n},easeInOutElastic:function(e,t,n,r,i){var s=1.70158;var o=0;var u=r;if(t==0)return n;if((t/=i/2)==2)return n+r;if(!o)o=i*.3*1.5;if(u<Math.abs(r)){u=r;var s=o/4}else var s=o/(2*Math.PI)*Math.asin(r/u);if(t<1)return-.5*u*Math.pow(2,10*(t-=1))*Math.sin((t*i-s)*2*Math.PI/o)+n;return u*Math.pow(2,-10*(t-=1))*Math.sin((t*i-s)*2*Math.PI/o)*.5+r+n},easeInBack:function(e,t,n,r,i,s){if(s==undefined)s=1.70158;return r*(t/=i)*t*((s+1)*t-s)+n},easeOutBack:function(e,t,n,r,i,s){if(s==undefined)s=1.70158;return r*((t=t/i-1)*t*((s+1)*t+s)+1)+n},easeInOutBack:function(e,t,n,r,i,s){if(s==undefined)s=1.70158;if((t/=i/2)<1)return r/2*t*t*(((s*=1.525)+1)*t-s)+n;return r/2*((t-=2)*t*(((s*=1.525)+1)*t+s)+2)+n},easeInBounce:function(e,t,n,r,i){return r-jQuery.easing.easeOutBounce(e,i-t,0,r,i)+n},easeOutBounce:function(e,t,n,r,i){if((t/=i)<1/2.75){return r*7.5625*t*t+n}else if(t<2/2.75){return r*(7.5625*(t-=1.5/2.75)*t+.75)+n}else if(t<2.5/2.75){return r*(7.5625*(t-=2.25/2.75)*t+.9375)+n}else{return r*(7.5625*(t-=2.625/2.75)*t+.984375)+n}},easeInOutBounce:function(e,t,n,r,i){if(t<i/2)return jQuery.easing.easeInBounce(e,t*2,0,r,i)*.5+n;return jQuery.easing.easeOutBounce(e,t*2-i,0,r,i)*.5+r*.5+n}});e.waitForImages={hasImageProperties:["backgroundImage","listStyleImage","borderImage","borderCornerImage"]};e.expr[":"].uncached=function(t){var n=document.createElement("img");n.src=t.src;return e(t).is('img[src!=""]')&&!n.complete};e.fn.waitForImages=function(t,n,r){if(e.isPlainObject(arguments[0])){n=t.each;r=t.waitForAll;t=t.finished}t=t||e.noop;n=n||e.noop;r=!!r;if(!e.isFunction(t)||!e.isFunction(n)){throw new TypeError("An invalid callback was supplied.")}return this.each(function(){var i=e(this),s=[];if(r){var o=e.waitForImages.hasImageProperties||[],u=/url\((['"]?)(.*?)\1\)/g;i.find("*").each(function(){var t=e(this);if(t.is("img:uncached")){s.push({src:t.attr("src"),element:t[0]})}e.each(o,function(e,n){var r=t.css(n);if(!r){return true}var i;while(i=u.exec(r)){s.push({src:i[2],element:t[0]})}})})}else{i.find("img:uncached").each(function(){s.push({src:this.src,element:this})})}var f=s.length,l=0;if(f==0){t.call(i[0])}e.each(s,function(r,s){var o=new Image;e(o).bind("load error",function(e){l++;n.call(s.element,l,f,e.type=="load");if(l==f){t.call(i[0]);return false}});o.src=s.src})})};
})(jQuery)

// SOME ERROR MESSAGES IN CASE THE PLUGIN CAN NOT BE LOADED
function revslider_showDoubleJqueryError(sliderID) {
	var errorMessage = "Revolution Slider Error: You have some jquery.js library include that comes after the revolution files js include.";
	errorMessage += "<br> This includes make eliminates the revolution slider libraries, and make it not work.";
	errorMessage += "<br><br> To fix it you can:<br>&nbsp;&nbsp;&nbsp; 1. In the Slider Settings -> Troubleshooting set option:  <strong><b>Put JS Includes To Body</b></strong> option to true.";
	errorMessage += "<br>&nbsp;&nbsp;&nbsp; 2. Find the double jquery.js include and remove it.";
	errorMessage = "<span style='font-size:16px;color:#BC0C06;'>" + errorMessage + "</span>"
		jQuery(sliderID).show().html(errorMessage);
}

(function(f,c){f.fn.extend({megafoliopro:function(m){var n={filterChangeAnimation:"rotatescale",filterChangeSpeed:400,filterChangeRotate:99,filterChangeScale:0.6,delay:20,defaultWidth:980,paddingHorizontal:10,paddingVertical:10,layoutarray:[11],lowSize:50,startFilter:"*"};m=f.extend({},n,m);return this.each(function(){if(!f.support.transition){f.fn.transition=f.fn.animate}var p=m;p.detaiview="off";p.firststart=1;if(p.filter==c){p.filter="*"}if(p.delay==c){p.delay=0}var q=p.firefox13=false;var r=p.ie=!f.support.opacity;var s=p.ie9=!f.support.htmlSerialize;if(r){f("body").addClass("ie8")}if(s){f("body").addClass("ie9")}var o=f(this);o.data("defaultwidth",p.defaultWidth);o.data("paddingh",p.paddingHorizontal);o.data("paddingv",p.paddingVertical);o.data("order",p.layoutarray);o.data("ie",r);o.data("ie9",s);o.data("ff",q);o.data("opt",p);a(o,p);l(o,0);h(o,"*");g(o);l(o,0);setTimeout(function(){j(o)},400);if(p.startFilter!="*"&&p.startFilter!=c){h(o,p.startFilter);l(o,0);j(o)}f(window).resize(function(){clearTimeout(o.data("resized"));o.data("resized",setTimeout(function(){i(o,0,o.find(">.event-box.tp-ordered").length)},150))});if((navigator.userAgent.match(/iPhone/i))||(navigator.userAgent.match(/iPod/i))||(navigator.userAgent.match(/iPad/i))){f(".event-box").click(function(){})}})},megamethode:function(m){return this.each(function(){var n=f(this)})},megagetcurrentorder:function(){var m=f(this);return m.data("lastorder")},megaappendentry:function(p){var n=f(this);var m=f(p);m.addClass("event-box-added");n.append(m);var o=n.data("opt");k(n,o);l(n,0);j(n,1);setTimeout(function(){h(n,o.filter);l(n,0);j(n,1)},200)},megaremix:function(m){return this.each(function(){var n=f(this);if(m!=c){n.data("order",m)}d(n);l(n,0);j(n)})},megafilter:function(m){return this.each(function(){var n=f(this);if(n.data("nofilterinaction")!=1){n.data("nofilterinaction",1);h(n,m);l(n,0);j(n);setTimeout(function(){n.data("nofilterinaction",0)},1200)}else{clearInterval(n.data("nextfiltertimer"));n.data("nextfiltertimer",setInterval(function(){if(n.data("nofilterinaction")!=1){clearInterval(n.data("nextfiltertimer"));h(n,m);l(n,0);j(n);n.data("nofilterinaction",1);setTimeout(function(){n.data("nofilterinaction",0)},1200)}},10))}})},megaanimchange:function(o,n,m,p){return this.each(function(){var q=f(this);var r=q.data("opt");var s=r.filter;r.filterChangeAnimation=o;r.filterChangeSpeed=n;r.filterChangeRotate=m;r.filterChangeScale=p;h(q,"");h(q,s);setTimeout(function(){l(q,0);j(q)},2*r.filterChangeSpeed);q.data("opt",r)})}});function a(m,n){m.find(">.event-box").each(function(){var p=f(this);p.removeClass("tp-layout-first-item").removeClass("tp-layout-last-item").removeClass("very-last-item");p.addClass("tp-notordered").addClass("event-box-added");p.wrapInner('<div class="event-box-innerwrap"><div></div></div>');var o=p.find(".event-box-innerwrap");o.css({background:"url("+p.data("src")+")",backgroundPosition:"50% 49%",backgroundSize:"cover","background-repeat":"no-repeat"});p.find(".mega-show-more").each(function(){var q=f(this);q.data("entid",p.attr("id"));q.click(function(){var r=f(this);var s=m.find("#"+r.data("entid"));s.addClass("mega-in-detailview");n.detailview="on"})})})}function k(m,n){m.find(">.event-box-added").each(function(p){var q=f(this);if(!q.hasClass("tp-layout")){q.removeClass("tp-layout-first-item").removeClass("tp-layout-last-item").removeClass("very-last-item");q.addClass("tp-notordered");q.wrapInner('<div class="event-box-innerwrap"></div>');var o=q.find(".event-box-innerwrap");o.css({background:"url("+q.data("src")+")",backgroundPosition:"50% 49%",backgroundSize:"cover","background-repeat":"no-repeat"});q.find(".mega-show-more").each(function(){var r=f(this);r.data("entid",q.attr("id"));r.click(function(){var s=f(this);var t=m.find("#"+s.data("entid"));t.addClass("mega-in-detailview");n.detailview="on"})})}})}function d(m){m.find(">.event-box.tp-layout").each(function(){var n=f(this);n.removeClass("tp-layout").addClass("tp-notordered")})}function h(m,p){var r=m.data("ie");var s=m.data("ie9");var o=m.data("opt");if(o.filterChangeSpeed==c){o.filterChangeSpeed=Math.round(Math.random()*500+100)}o.filter=p;var q=1;var n=1;m.find(">.event-box").each(function(x){var z=f(this);var t=o.filterChangeRotate;if(t==c){t=30}if(t==99){t=Math.round(Math.random()*50-25)}z.removeClass("tp-layout-first-item").removeClass("tp-layout-last-item").removeClass("very-last-item");var y=p.split(",");var w=false;for(var v=0;v<y.length;v++){if(z.hasClass(y[v])){w=true;/*console.log("has class")*/}}if(w||p=="*"){z.removeClass("tp-layout").addClass("tp-notordered")}else{z.removeClass("tp-ordered").removeClass("tp-layout");setTimeout(function(){if(r||s){z.animate({scale:0,opacity:0},{queue:false,duration:o.filterChangeSpeed})}else{if(o.filterChangeAnimation=="fade"){z.transition({scale:1,opacity:0,rotate:0},o.filterChangeSpeed);z.find(".event-box-innerwrap").transition({scale:1,opacity:1,perspective:"10000px",rotateX:"0deg"},o.filterChangeSpeed)}else{if(o.filterChangeAnimation=="scale"){z.transition({scale:o.filterChangeScale,opacity:0,rotate:0},o.filterChangeSpeed);z.find(".event-box-innerwrap").transition({scale:1,opacity:1,perspective:"10000px",rotateX:"0deg"},o.filterChangeSpeed)}else{if(o.filterChangeAnimation=="rotate"){z.transition({scale:1,opacity:0,rotate:t},o.filterChangeSpeed);z.find(".event-box-innerwrap").transition({scale:1,opacity:1,perspective:"10000px",rotateX:"0deg"},o.filterChangeSpeed)}else{if(o.filterChangeAnimation=="rotatescale"){z.transition({scale:o.filterChangeScale,opacity:0,rotate:t},o.filterChangeSpeed);z.find(".event-box-innerwrap").transition({scale:1,opacity:1,perspective:"10000px",rotateX:"0deg"},o.filterChangeSpeed)}else{if(o.filterChangeAnimation=="pagetop"||o.filterChangeAnimation=="pagebottom"||o.filterChangeAnimation=="pagemiddle"){z.find(".event-box-innerwrap").removeClass("pagemiddle").removeClass("pagetop").removeClass("pagebottom").addClass(o.filterChangeAnimation);z.transition({opacity:0},o.filterChangeSpeed);z.find(".event-box-innerwrap").transition({scale:1,opacity:0,perspective:"10000px",rotateX:"90deg"},o.filterChangeSpeed)}}}}}}setTimeout(function(){z.css({visibility:"hidden"})},o.filterChangeSpeed)},n*o.delay/2);n++}})}function g(m){var q=m.data("ie");var r=m.data("ie9");var o=m.data("opt");if(o.filterChangeSpeed==c){o.filterChangeSpeed=Math.round(Math.random()*500+100)}if(o.filterChangeScale==c){o.filterChangeScale=0.8}var p=0;var n=0;m.find(">.event-box").each(function(t){var u=f(this);var s=o.filterChangeRotate;if(s==c){s=30}if(s==99){s=Math.round(Math.random()*360)}if(q||r){u.css({opacity:0})}else{if(o.filterChangeAnimation=="fade"){u.transition({scale:1,opacity:0,rotate:0,duration:1,queue:false})}else{if(o.filterChangeAnimation=="scale"){u.transition({scale:o.filterChangeScale,opacity:0,rotate:0,duration:1,queue:false})}else{if(o.filterChangeAnimation=="rotate"){u.transition({scale:1,opacity:0,rotate:s,duration:1,queue:false})}else{if(o.filterChangeAnimation=="rotatescale"){u.transition({scale:o.filterChangeScale,opacity:0,rotate:s,duration:1,queue:false})}else{if(o.filterChangeAnimation=="pagetop"||o.filterChangeAnimation=="pagebottom"||o.filterChangeAnimation=="pagemiddle"){u.find(".event-box-innerwrap").addClass(o.filterChangeAnimation);u.transition({opacity:0,duration:1,queue:false});u.find(".event-box-innerwrap").transition({scale:1,opacity:1,perspective:"10000px",rotateX:"90deg",duration:1,queue:false})}}}}}}})}function l(o,y){if(y==0){var s=new Array()}else{var s=o.data("lastorder")}var r=o.width();var q=o.data("order");if(y>q.length-1){y=0}var t=o.find(">.event-box.tp-notordered");var m=12;if(t.length<9){m=t.length}var x=q[y];var v=false;var n=!f.support.opacity;var p=!f.support.htmlSerialize;if(q[y]==0||x<2||x>23){if(n){x=9}else{x=Math.round(Math.random()*m+1)}}if(x<2){x=2}if(x>23){x=23}s.push(x);var w=x;if(x==10||x==14){w=3}if(x==11||x==15){w=4}if(x==12||x==16){w=5}if(x==13||x==17){w=6}if(x==11||x==12||x==13||x==15||x==16||x==17){if(r<840&&r>721){w=4}else{if(r<720){w=3}}}if(x==18||x==19||x==20){w=1}if(x==21||x==22||x==23){w=2}t.slice(0,w).each(function(z){var A=f(this);A.removeClass("tp-layout-first-item").removeClass("tp-layout-last-item").removeClass("very-last-item");A.addClass("tp-ordered tp-layout");A.data("layout",x);A.data("child",z);if(z==0){A.addClass("tp-layout-first-item")}if(z==w-1){A.addClass("tp-layout-last-item")}A.removeClass("tp-notordered")});y=y+1;o.data("lastorder",s);if(o.find(">.event-box.tp-notordered").length>0){l(o,y)}else{try{b(o).addClass("very-last-item")}catch(u){}return o}}function i(o,y,v){if(y==0){var s=new Array()}else{var s=o.data("lastorder")}var r=o.width();var q=o.data("order");if(y>q.length-1){y=0}var t=o.find(">.event-box.tp-ordered");var m=12;if(t.length<9){m=t.length}var x=q[y];var u=false;var n=!f.support.opacity;var p=!f.support.htmlSerialize;if(q[y]==0||x<2||x>23){if(n){x=9}else{x=Math.round(Math.random()*m+1)}}if(x<2){x=2}if(x>23){x=23}s.push(x);var w=x;if(x==10||x==14){w=3}if(x==11||x==15){w=4}if(x==12||x==16){w=5}if(x==13||x==17){w=6}if(x==11||x==12||x==13||x==15||x==16||x==17){if(r<840&&r>721){w=4}else{if(r<720){w=3}}}if(x==18||x==19||x==20){w=1}if(x==21||x==22||x==23){w=2}var z=t.length-v;t.slice(z,z+w).each(function(A){var B=f(this);B.removeClass("tp-layout-first-item").removeClass("tp-layout-last-item").removeClass("very-last-item");B.addClass("tp-ordered tp-layout");B.data("layout",x);B.data("child",A);if(A==0){B.addClass("tp-layout-first-item")}if(A==w-1){B.addClass("tp-layout-last-item")}B.removeClass("tp-notordered")});y=y+1;o.data("lastorder",s);v=v-w;if(v>0){i(o,y,v)}else{b(o).addClass("very-last-item");j(o);return o}}function b(m){var n;m.find(">.event-box.tp-layout.tp-ordered").each(function(){n=f(this)});return n}function e(m){return m}function j(ak,L){var r=0;var v=ak.data("defaultwidth");var O=ak.data("opt");var Z=O.delay;var W=0;if(O.firststart==1){W=1;O.firststart=0}var T=ak.width();var N=T/v;var S=185;var ay=ak.data("paddingh");var ar=ak.data("paddingv");var u=1;var aw=1;var al=ak.data("ie");var F=ak.data("ie9");var aj=5*N;var w=0;var X=T;var M=e(aj*160);var an=e(aj*158);var J=e(aj*152);var n=e(aj*148);var V=e(aj*132);var s=e(aj*123);var y=e(aj*122);var ad=e(aj*119);var ac=e(aj*113);var af=e(aj*112);var ao=e(aj*98);var o=e(aj*84);var G=e(aj*83);var at=e(aj*79);var av=e(aj*78);var z=e(aj*77);var aa=e(aj*74);var aq=e(aj*73);var Q=e(aj*69);var U=e(aj*68);var ap=e(aj*67);var t=e(aj*65.3);var ab=e(aj*49);var ae=e(aj*48);var B=e(aj*45);var E=e(aj*44);var m=e(aj*39.2);var q=e(aj*39);var x=e(aj*38);var I=e(aj*37);var P=e(aj*36);var p=e(aj*32.66);var Y=e(aj*222);var ax=n;var am=e(aj*111);var H=aa;var ai=aq;var K=ab;var R=ae;var A=I;var D=P;var au=new Array(0,0,0,0,0,0,0,0,0);var C=new Array(0,0,0,0,0,0,0,0,0);var ag=0;var ah=0;var az=ak.find(">.event-box.tp-layout").length;ak.find(">.event-box.tp-layout").each(function(aT){var aX=f(this);var aC=aX.find(".event-box-innerwrap");ah=aX.data("layout");if(ah==11||ah==12||ah==13){if(T<840&&T>721){ah=11}else{if(T<720){ah=10}}}if(ah==15||ah==16||ah==17){if(T<840&&T>721){ah=15}else{if(T<720){ah=14}}}var aS=500;var aH,aU,aI;var a0=ay;var aN=ar;var aV=r;var a1=A;if(T>480){if(ah==2){if(T>767){if(aX.data("child")==0){aH=ac;aU=H;aI=0;a1=H}if(aX.data("child")==1){aH=G;aU=H;aI=ac;a0=0;r=r+H;a1=H}}else{if(T>480&&T<768){if(aX.data("child")==0){aH=o;aU=H;aI=0;a1=H}if(aX.data("child")==1){aH=af;aU=H;aI=o;a0=0;r=r+H;a1=H}}}}if(ah==3){if(T>767){if(aX.data("child")==0){aH=av;aU=H;aI=0;a1=H}if(aX.data("child")==1){aH=B;aU=H;aI=av;a1=H}if(aX.data("child")==2){aH=aq;aU=H;aI=s;r=r+H;a1=H;a0=0}}else{if(T>480&&T<768){if(aX.data("child")==0){aH=aa;aU=H;aI=0;a1=H}if(aX.data("child")==1){aH=av;aU=H;aI=aa;a1=H}if(aX.data("child")==2){aH=E;aU=H;aI=J;a1=H;r=r+H;a0=0}}}}if(ah==4){if(T>767){if(aX.data("child")==0){aH=o;aU=H;aI=0;a1=370}if(aX.data("child")==1){aH=q;aU=A;aI=o}if(aX.data("child")==2){aH=aq;aU=H;aI=s;a0=0;r=r+A;a1=H}if(aX.data("child")==3){aH=q;aU=A;aI=o;r=r+A}}else{if(T>480&&T<768){if(aX.data("child")==0){aH=q;aU=A;aI=o}if(aX.data("child")==1){aH=o;aU=H;aI=0;r=r+A}if(aX.data("child")==2){aH=q;aU=A;aI=o;r=r-A}if(aX.data("child")==3){aH=aq;aU=H;aI=s;a0=0;r=r+H;a1=H}}}}if(ah==5){if(T>767){if(aX.data("child")==0){aH=o;aU=A;aI=0}if(aX.data("child")==1){aH=q;aU=A;aI=o}if(aX.data("child")==2){aH=aq;aU=H;aI=s;a0=0;r=r+A;a1=H}if(aX.data("child")==3){aH=E;aU=A;aI=0}if(aX.data("child")==4){aH=at;aU=A;aI=E;r=r+A}}else{if(T>480&&T<768){if(aX.data("child")==0){aH=o;aU=A;aI=0}if(aX.data("child")==1){aH=q;aU=A;aI=o}if(aX.data("child")==2){aH=aq;aU=A;aI=s;a0=0;r=r+A}if(aX.data("child")==3){aH=ao;aU=A;aI=0}if(aX.data("child")==4){aH=ao;aU=A;aI=ao;a0=0;r=r+A}}}}if(ah==6){if(T>767){if(aX.data("child")==0){aH=aa;aU=H;aI=0;a1=H}if(aX.data("child")==1){aH=B;aU=A;aI=aa}if(aX.data("child")==2){aH=z;aU=A;aI=ad;a0=0;r=r+A}if(aX.data("child")==3){aH=B;aU=A;aI=aa}if(aX.data("child")==4){aH=q;aU=A;aI=ad}if(aX.data("child")==5){aH=x;aU=A;aI=an;a0=0;r=r+A}}else{if(T>480&&T<768){if(aX.data("child")==0){aH=B;aU=H;aI=0;a1=H}if(aX.data("child")==1){aH=U;aU=H;aI=B;a1=H}if(aX.data("child")==2){aH=q;aU=A;aI=ac}if(aX.data("child")==3){aH=E;aU=A;aI=J;a0=0;r=r+A}if(aX.data("child")==4){aH=q;aU=A;aI=ac}if(aX.data("child")==5){aH=E;aU=A;aI=J;a0=0;r=r+A}}}}if(ah==7){if(T>767){if(aX.data("child")==0){aH=ac;aU=H;aI=0;a1=H}if(aX.data("child")==1){aH=G;aU=A;aI=ac;a0=0;r=r+A}if(aX.data("child")==2){aH=q;aU=A;aI=ac}if(aX.data("child")==3){aH=E;aU=H;aI=J;a0=0;r=r+A;a1=H}if(aX.data("child")==4){aH=B;aU=A;aI=0}if(aX.data("child")==5){aH=U;aU=A;aI=B}if(aX.data("child")==6){aH=q;aU=A;aI=ac;r=r+A}}else{if(T>480&&T<768){if(aX.data("child")==0){aH=ac;aU=H;aI=0;a1=H}if(aX.data("child")==1){aH=G;aU=A;aI=ac;a0=0;r=r+A}if(aX.data("child")==2){aH=q;aU=A;aI=ac}if(aX.data("child")==3){aH=E;aU=A;aI=J;a0=0;r=r+A}if(aX.data("child")==4){aH=B;aU=A;aI=0}if(aX.data("child")==5){aH=U;aU=A;aI=B}if(aX.data("child")==6){aH=G;aU=A;aI=ac;a0=0;r=r+A}}}}if(ah==8){if(T>767){if(aX.data("child")==0){aH=G;aU=A;aI=0}if(aX.data("child")==1){aH=Q;aU=H;aI=G;a1=H}if(aX.data("child")==2){aH=E;aU=A;aI=J;a0=0;r=r+A}if(aX.data("child")==3){aH=q;aU=A;aI=0}if(aX.data("child")==4){aH=E;aU=A;aI=q}if(aX.data("child")==5){aH=E;aU=H;aI=J;a0=0;r=r+A;a1=A}if(aX.data("child")==6){aH=G;aU=A;aI=0}if(aX.data("child")==7){aH=Q;aU=A;aI=G;r=r+A}}else{if(T>480&&T<768){if(aX.data("child")==0){aH=G;aU=A;aI=0}if(aX.data("child")==1){aH=q;aU=A;aI=G}if(aX.data("child")==2){aH=aa;aU=A;aI=y;a0=0;r=r+A;a1=H}if(aX.data("child")==3){aH=q;aU=A;aI=0}if(aX.data("child")==4){aH=G;aU=A;aI=q}if(aX.data("child")==5){aH=aa;aU=H;aI=y;a0=0;r=r+A;a1=H}if(aX.data("child")==6){aH=G;aU=A;aI=0}if(aX.data("child")==7){aH=q;aU=A;aI=G;r=r+A}}}}if(ah==9){if(T>767){if(aX.data("child")==0){aH=ac;aU=H;aI=0;a1=H}if(aX.data("child")==1){aH=G;aU=A;aI=ac;a0=0;r=r+A}if(aX.data("child")==2){aH=q;aU=A;aI=ac}if(aX.data("child")==3){aH=E;aU=H;aI=J;a0=0;r=r+A;a1=H}if(aX.data("child")==4){aH=B;aU=H;aI=0;a1=H}if(aX.data("child")==5){aH=U;aU=H;aI=B;a1=H}if(aX.data("child")==6){aH=q;aU=A;aI=ac;r=r+A}if(aX.data("child")==7){aH=q;aU=A;aI=ac}if(aX.data("child")==8){aH=E;aU=A;aI=J;a0=0;r=r+A}}else{if(T>480&&T<768){if(aX.data("child")==0){aH=aa;aU=H;aI=0;a1=H}if(aX.data("child")==1){aH=ae;aU=H;aI=aa;a1=H}if(aX.data("child")==2){aH=aa;aU=H;aI=y;a0=0;r=r+H;a1=H}if(aX.data("child")==3){aH=ae;aU=H;aI=0;a1=H}if(aX.data("child")==4){aH=aa;aU=H;aI=ae;a1=H}if(aX.data("child")==5){aH=aa;aU=H;aI=y;a0=0;r=r+H;a1=H}if(aX.data("child")==6){aH=aa;aU=H;aI=0;a1=H}if(aX.data("child")==7){aH=aa;aU=H;aI=aa;a1=H}if(aX.data("child")==8){aH=ae;aU=H;aI=n;a0=0;r=r+H;a1=H}}}}if(ah>9&&ah<14){if(ah==10){aH=Math.round(t);aH=aH+(a0/3)}else{if(ah==11){aH=Math.round(ab);aH=aH+(a0/4)}else{if(ah==12){aH=Math.round(m);aH=aH+(a0/5)}else{if(ah==13){aH=Math.round(p);aH=aH+(a0/6)}}}}var aU=aH;var aR=aX.data("child");aI=aH*aR;a1=aU;if((aR==2&&ah==10)||(aR==3&&ah==11)||(aR==4&&ah==12)||(aR==5&&ah==13)||(aX.hasClass("tp-layout-last-item"))){r=r+aU}}if(ah==18){aH=Math.round(X);aH=aH+(a0);var aU=aH;var aR=aX.data("child");aI=aH*aR;a1=aU;r=r+aU}if(ah==19){aH=Math.round(X);aH=aH+(a0);var aU=aH/2;var aR=aX.data("child");aI=aH*aR;a1=aU;r=r+aU}if(ah==20){aH=Math.round(X);aH=aH+(a0);var aU=aH/3;var aR=aX.data("child");aI=aH*aR;a1=aU;r=r+aU}if(ah==21){aH=Math.round(ao);aH=aH+(a0/2);var aU=aH;var aR=aX.data("child");aI=aH*aR;a1=aU;if(aR==1){r=r+aU}}if(ah==22){aH=Math.round(ao);aH=aH+(a0/2);var aU=aH/2;var aR=aX.data("child");aI=aH*aR;a1=aU;if(aR==1){r=r+aU}}if(ah==23){aH=Math.round(ao);aH=aH+(a0/2);var aU=aH/3;var aR=aX.data("child");aI=aH*aR;a1=aU;if(aR==1){r=r+aU}}if(ah>13&&ah<18){if(ah==14){aH=Math.round(t);aH=aH+(a0/3)}else{if(ah==15){aH=Math.round(ab);aH=aH+(a0/4)}else{if(ah==16){aH=Math.round(m);aH=aH+(a0/5)}else{if(ah==17){aH=Math.round(p);aH=aH+(a0/6)}}}}var aR=aX.data("child");var aA=aH/aX.data("width");aU=aX.data("height")*aA;aI=aH*aX.data("child");aV=au[aR];a1=aU*aA;C[aR]=a1;r=aV+aU;au[aR]=r}}else{aU=Math.round(aX.data("height")*(T/aX.data("width")));aH=T;a0=0;aI=0;aV=r;r=r+aU}var aO=1;var aG=1;var aE=0;var aF=0;var aZ=O.filterChangeRotate;if(aZ==c){aF=30}if(aZ==99){aZ=Math.round(Math.random()*360)}var aQ=O.filter.split(",");var aJ=false;for(var aK=0;aK<aQ.length;aK++){if(aX.hasClass(aQ[aK])){aJ=true;/*console.log("has class")*/}}if(aJ||O.filter=="*"){aX.css({visibility:"visible"});if(al||F){aO=1;aG=1}else{if(O.filterChangeAnimation=="pagetop"||O.filterChangeAnimation=="pagebottom"||O.filterChangeAnimation=="pagemiddle"){aF=0;aE=0;aO=1;aG=1;eiscal=1;eiopaa=1;eirx=0}else{aO=1;aF=0;aG=1;eiscal=1;eiopaa=1;eirx=0}}}if(O.detailview=="on"&&aG==1){aG=0.4}if(aX.hasClass("mega-in-detailview")){aG=1}aX.removeClass("mega-square").removeClass("mega-portrait").removeClass("mega-landscape");var aL=Math.floor(aH/100);var aP=Math.floor(aU/100);if(aL>aP){aX.addClass("mega-landscape")}if(aP>aL){aX.addClass("mega-portrait")}if(aL==aP){aX.addClass("mega-square")}var aB=aT*O.delay;if(al||F){aX.find(".mega-socialbar").animate({width:aH+"px"});aX.animate({scale:aO,opacity:aG,width:aH+"px",height:aU+"px",left:aI+"px",top:aV+"px",paddingBottom:aN+"px",paddingRight:a0+"px"},{queue:false,duration:400});aC.animate({"background-position":"50% 49%","background-size":"cover"},{queue:false,duration:400});if(al){var a2=aC.find(".ieimg");var aW=Math.round(aX.data("width"))/Math.round(aX.data("height"));var aM=Math.round(aH)/Math.round(aU);var aY=aH;var aD=aY/aX.data("width")*aX.data("height");if(aD<aU){aD=aU;aY=aD/aX.data("height")*aX.data("width")}a2.css({width:aY+"px",height:aD+"px"})}}else{var aA=(T/v)*100-16;if(aX.data("lowsize")!=c){if(aA<=aX.data("lowsize")){aX.addClass("mega-lowsize")}else{aX.removeClass("mega-lowsize")}}if(W){aB=aB+100;aX.transition({opacity:0,top:aV+"px",left:aI+"px",width:aH,height:aU,paddingBottom:aN+"px",paddingRight:a0+"px",duration:1,queue:false})}setTimeout(function(){aX.transition({scale:aO,opacity:aG,rotate:aF,"z-index":1,width:aH,height:aU,top:aV+"px",left:aI+"px",paddingBottom:aN+"px",paddingRight:a0+"px",duration:O.filterChangeSpeed,queue:false});setTimeout(function(){aX.find(".event-box-innerwrap").transition({scale:eiscal,opacity:eiopaa,perspective:"10000px",rotateX:eirx,duration:O.filterChangeSpeed,queue:false});aX.removeClass("event-box-added")},50);aC.transition({"background-position":"50% 49%","background-size":"cover",duration:O.filterChangeSpeed,queue:false})},aB)}if(aX.hasClass("very-last-item")&&!aX.hasClass("tp-layout-last-item")){r=r+a1}else{}if(w<aV+aU){w=aV+aU}});if(ah>13&&ah<18){r=au[0];for(allh=0;allh<au.length;allh++){if(r<au[allh]){r=au[allh]}}}ak.css({height:w+"px"})}})(jQuery);

function setEventBoxesDesign()
{
    var api = jQuery('.event-boxes-container').megafoliopro({
        filterChangeAnimation: (ozyMegafolioOptions.filterChangeAnimation == null ? 'scale' : ozyMegafolioOptions.filterChangeAnimation), // fade, rotate, scale, rotatescale, pagetop, pagebottom,pagemiddle
        filterChangeSpeed: 400,
        filterChangeRotate: 99,
        filterChangeScale: 0.6,
        delay: 20,
        defaultWidth: 980,
        paddingHorizontal: (ozyMegafolioOptions.padding == null || ozyMegafolioOptions.padding == '' ? 1 : ozyMegafolioOptions.padding),
        paddingVertical: (ozyMegafolioOptions.padding == null || ozyMegafolioOptions.padding == '' ? 1 : ozyMegafolioOptions.padding),
        layoutarray: (ozyMegafolioOptions.layoutarray == null ? '9' : ozyMegafolioOptions.layoutarray).split(',').map(Number)
    });
    jQuery('.mega-socialbar,.event-box-content,.load_more_blog').fadeIn();
    jQuery('.load_more_blog').css('display', 'block');
}

jQuery(document).ready(function() {

    setEventBoxesDesign();

	//FIX LOADING FLICKER ISSUE
	
});


function onEventsLoadSuccess()
{
    setEventBoxesDesign();
}

function onEventsLoadFailure()
{
    return "Error in loading the events";
}

function onUserEventsLoadSuccess() {
    setEventBoxesDesign();
    $("#eventsWall").css("display", "block");
    $("#loader").css("display", "none");
}