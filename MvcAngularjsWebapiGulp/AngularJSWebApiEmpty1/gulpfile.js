var gulp = require('gulp'),
    connect = require('gulp-connect'),
    plug = require('gulp-load-plugins')();
var watch = require('gulp-watch');
var browserSync = require('browser-sync').create();

var colors = plug.util.colors,
    env = plug.util.env;
    log = plug.util.log;

var paths = require('./gulp.config.json');

gulp.task('livereload', function () {
    gulp.src(['src/client/content/**/*.css', 'src/client/app/**/*.js', 'src/client/app/**/*.html'])
        .pipe(plug.watch(['src/client/content/**/*.css', 'src/client/app/**/*.js', 'src/client/app/**/*.html']))
        .pipe(connect.reload());
});

/**
 * Create $templateCache from the html templates
 * @return {Stream}
 */
gulp.task('templatecache', function() {
    log('Creating an AngularJS $templateCache');

    return gulp
        .src(paths.htmltemplates)
//        .src('./src/client/app/layout/*.html')
        // .pipe(plug.bytediff.start())
        .pipe(plug.minifyHtml({
            empty: true
        }))
        // .pipe(plug.bytediff.stop(bytediffFormatter))
        .pipe(plug.angularTemplatecache('templates.js', {
            module: 'app.core',
            standalone: false,
            root: 'app/'
        }))
        .pipe(gulp.dest(paths.build));
});


/**
 * Copy the Vendor JavaScript
 * @return {Stream}
 */
gulp.task('vendorjs', function() {
    console.log('Bundling, minifying, and copying the Vendor JavaScript');

    return gulp.src(paths.vendorjs)
        .pipe(plug.concat('vendor.min.js'))
        .pipe(plug.uglify())
        .pipe(gulp.dest(paths.build));
});


/**
 * Minify and bundle the app's JavaScript
 * @return {Stream}
 */
gulp.task('js', ['templatecache'], function() {
    console.log('Bundling, minifying, and copying the app\'s JavaScript');

    var source = [].concat(paths.js, paths.build + 'templates.js');
    
    return gulp
        .src(source)
        // .pipe(plug.sourcemaps.init()) // get screwed up in the file rev process
        .pipe(plug.concat('all.min.js'))
        .pipe(plug.ngAnnotate({
            add: true,
            single_quotes: true
        }))
        .pipe(plug.uglify({
            mangle: true
        }))
        .pipe(gulp.dest(paths.build));
});

/**
 * Copy fonts
 * @return {Stream}
 */
gulp.task('fonts', function() {
    var dest = paths.build + 'fonts';
    log('Copying fonts');
    return gulp
        .src(paths.fonts)
        .pipe(gulp.dest(dest));
});

/**
 * Inject all the files into the new index.html
 * rev, but no map
 * @return {Stream}
 */
gulp.task('rev-and-inject', ['js', 'vendorjs', 'css', 'vendorcss'], function() {
//gulp.task('rev-and-inject', function() {
    console.log('Rev\'ing files and building index.html');

    var minified = paths.build + '**/*.min.*';
    var index = paths.client + 'index.html';
    var minFilter = plug.filter(['**/*.min.*', '!**/*.map']);
    var indexFilter = plug.filter(['index.html']);

    //console.log([].concat(minified, index));
    //console.log(indexFilter);
    var stream = gulp
        // Write the revisioned files
        .src([].concat(minified, index)) // add all built min files and index.html
        .pipe(minFilter) // filter the stream to minified css and js
        .pipe(plug.rev()) // create files with rev's
        .pipe(gulp.dest(paths.build)) // write the rev files
        .pipe(minFilter.restore()) // remove filter, back to original stream

//    // inject the files into index.html
        .pipe(indexFilter) // filter to index.html
        .pipe(inject('content/vendor.min.css', 'inject-vendor'))
        .pipe(inject('content/all.min.css'))
        .pipe(inject('vendor.min.js', 'inject-vendor'))
        .pipe(inject('all.min.js'))
        .pipe(gulp.dest(paths.build)) // write the rev files
        .pipe(indexFilter.restore()) // remove filter, back to original stream
//
//    // replace the files referenced in index.html with the rev'd files
        .pipe(plug.revReplace()) // Substitute in new filenames
        .pipe(gulp.dest(paths.build)) // write the index.html file changes
        .pipe(plug.rev.manifest()) // create the manifest (must happen last or we screw up the injection)
        .pipe(gulp.dest(paths.build)); // write the manifest
//
    function inject(path, name) {
        var pathGlob = paths.build + path;
        var options = {
            ignorePath: paths.build.substring(1),
            read: false,
            addRootSlash: false,
            addPrefix: '.'
        };
        console.log(options.ignorePath);
        if (name) {
            options.name = name;
        }
        return plug.inject(gulp.src(pathGlob), options);
    }
});


gulp.task('default', ['serve-dev']);
gulp.task('serve-dev-livereload', ['serve-dev', 'livereload']);

/**
 * Build the optimized app
 * @return {Stream}
 */
gulp.task('build', ['rev-and-inject', 'templatecache', 'fonts'], function() {
    console.log('Building the optimized app');

//    return gulp.src('').pipe(plug.notify({
//        onLast: true,
//        message: 'Deployed code!'
//    }));
});

/**
 * Build development the optimized app
 * @return {Stream}
 */
gulp.task('build-dev', ['clean-dev'], function() {
    console.log('Building the optimized app');

//    return gulp.src('').pipe(plug.notify({
//        onLast: true,
//        message: 'Deployed developnet code!'
//    }));
});

/**
 * serve the dev environment
 */
gulp.task('serve-dev', function() {
    connect.server({
        root: [paths.client, './'],
        livereload: true
    });
});

/**
 * serve the build environment
 */
gulp.task('serve-build', function() {
    connect.server({
        root: "./build",
        livereload: true
    });
});


// browser-sync Static server, monitor all js, html, css files then reload page 
gulp.task('browser-sync', function() {
    browserSync.init({
        server: {
            baseDir: [paths.client, './']
        },
        files: ['src/client/app/**/*.html', 'src/client/app/**/*.css', 'src/client/app/**/*.js']
    });
});

/**
 * Formatter for bytediff to display the size changes after processing
 * @param  {Object} data - byte data
 * @return {String}      Difference in bytes, formatted
 */
function bytediffFormatter(data) {
    var difference = (data.savings > 0) ? ' smaller.' : ' larger.';
    return data.fileName + ' went from ' +
        (data.startSize / 1000).toFixed(2) + ' kB to ' + (data.endSize / 1000).toFixed(2) + ' kB' +
        ' and is ' + formatPercent(1 - data.percent, 2) + '%' + difference;
}

/**
 * Format a number as a percentage
 * @param  {Number} num       Number to format as a percent
 * @param  {Number} precision Precision of the decimal
 * @return {String}           Formatted percentage
 */
function formatPercent(num, precision) {
    return (num * 100).toFixed(precision);
}