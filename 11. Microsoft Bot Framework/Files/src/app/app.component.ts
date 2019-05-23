import { Component, OnInit } from '@angular/core';
import { Router, NavigationStart } from '@angular/router';

declare var $: any;
declare var _: any;
declare var fakeLoader: any;

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})

export class AppComponent {

  public layout: string = 'collapsed-sidebar-1'; //default-sidebar-1,off-canvas-1,sidebar-over-1,top-navigation-1,top-navigation-2,empty-view-1
  public background: string = 'light';//light,dark,indigo,blue-grey
  public navbar: string = 'blue-grey';//light,dark,indigo,blue-grey
  public sidebar: string = 'blue-grey';//light,dark,indigo,blue-grey
  public topNavigation: string = 'light';//light,dark,indigo,blue-grey
  public logo: string = 'light';//light,dark,indigo,blue-grey
  public collapsed: boolean = false;//true,false
  public controller: string;
  public view: string;

  constructor(private router: Router) {

    let config = {
      layout: this.layout,
      background: this.background,
      navbar: this.navbar,
      sidebar: this.sidebar,
      topNavigation: this.topNavigation,
      collapsed: this.collapsed,
      logo: this.logo
    };

    //set attributes before page is loaded. this can be removed in production
    //alternatively you can set them in the index.html
    $('body').attr('data-background', config.background);
    $('body').attr('data-navbar', config.navbar);
    $('body').attr('data-sidebar', config.sidebar);
    $('body').attr('data-top-navigation', config.topNavigation);
    $('body').attr('data-collapsed', config.collapsed);
    $('body').attr('data-logo', config.logo);

    if ($('html').hasClass('loading')) {
      const loaderTime = 2000;
      $('#fakeloader').fakeLoader({
        timeToHide: loaderTime,
        zIndex: '99999',
        spinner: 'spinner7',
        bgColor: '#263238'
      });
      setTimeout(function () {
        $('html').removeClass('loading');
      }, loaderTime);
    }

    const emptyView1 = [
      '/pages/error-page',
    ];

    const self = this;

    router.events.subscribe((val) => {
      if (val instanceof NavigationStart) {

        const copy = Object.assign({}, val);
        const url = copy.url;

        if (_.includes(emptyView1, url)) {
          self['layout'] = 'empty-view-1';
          $('body').attr('data-background', 'light');
          $('body').attr('data-layout', self['layout']);
        } else {
          self['layout'] = config.layout;
          $('body').attr('data-layout', self['layout']);
        }

        //set data-controller and data-view attributes
        const data = url.split('/').filter(url => url.length > 0);
        let currentController = $('body').attr('data-controller');
        let currentView = $('body').attr('data-view');

        if (data.length == 1) {
          self['layout'] = data[0];
          $('body').attr('data-layout', data[0]);
          $('body').attr('data-collapsed', false);
          config = Object.assign({}, config, {
            layout: data[0],
            collapsed: false
          });
          let newUrl = `/${currentController}/${currentView}`;
          router.navigateByUrl(newUrl);

        } else if (data.length == 2) {
          $('body').attr('data-controller', data[0]);
          $('body').attr('data-view', data[1]);
          self['controller'] = data[0];
          self['view'] = data[1];
        } else {
          self['controller'] = 'dashboards';
          self['view'] = 'dashboard';
          $('body').attr('data-controller', 'dashboards');
          $('body').attr('data-view', 'dashboard');
        }

      }
    });

  }

}

