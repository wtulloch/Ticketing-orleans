import { customElement, LitElement, CSSResult, css, TemplateResult, html, property } from "lit-element";
import {TicketingServices} from '../services/ticketing-service'
import { IShowInfo } from '../models/show-info.model';

@customElement('home-view')
export class HomeView extends LitElement {

    private service: TicketingServices;

    @property({type:Array})
     shows: string[] | undefined = undefined;
    
    @property({type: Object})
    showInfo: IShowInfo = {
        showId: '',
        tickets: [],
        unsoldTickets:[]
    };

    static get styles(): CSSResult {
        return css `
            :host {
               display: flex;
                
            }

            .show-container {
                display: flex;
                flex-direction: column;
                margin-left: 10px;
                margin-right: 10px;
            }
            .show-card {
                display:flex;
                width: 200px;
                height: 60px;
                margin: 10px;
                border: 1px solid black;
                align-content: center;
                justify-content: center;
                background-color: darkOrange;
                opacity: 0.8;

            }
            .show-card:hover {
                opacity: 1;
                cursor: pointer;
            }
            .current-show-container {
                display: flex;
                flex-direction: column;
            }
            .ticket-container {
                display: flex;
               flex-flow: row wrap;
            }

            .ticket-card {
                width: 100px;
                height: 100px;
                display: flex;
                justify-content: center;
                align-content: center;
                border: 1px solid black;
                margin: 5px;
                background-color: #3cd63c;
                opacity: 0.8;
            }

            .ticket-card:hover {
                cursor: pointer;
                opacity: 1;
            }

            .ticket-sold {
                background-color: red;
            }



        `;
    }

    render(): TemplateResult {
        return html `
            <div class="show-container">
               
                ${this.shows ? 
                    this.shows.map(s => html `<div class="show-card" @click=${() => this.getShowTickets(s)} ><p>${s}</p></div>`)
                    : html`<div class="show-card"> No shows yet</div>`    
            }
               
            </div>
            <div class="current-show-container">
        <h2>${this.showInfo.showId}</h2>
        <div class="ticket-container">
        ${this.showInfo.tickets != null ?
            this.showInfo.tickets.map(t => {
                if(t.sold) {
                   return html `<div class="ticket-card ticket-sold" ><p>${t.ticketId}</p></div>`
                }
               return html `<div class="ticket-card" @click=${() => this.purchaseTicket(this.showInfo.showId,t.ticketId)}><p>${t.ticketId}</p></div>`
            })
            : html ``
        }
        </div>
            </div>
        `;
    }

  /**
   *
   */
  constructor() {
      super();
      this.service =   new TicketingServices();
  }

    createRenderRoot(){
        const root = super.createRenderRoot();
        return root;
    }

   async connectedCallback() {
        super.connectedCallback();
        await this.getShows()
    }

    disconnectedCallback() {
        super.disconnectedCallback();
    }

    async getShows() {
       

       this.shows = await this.service.getShows();
       
    }

    async getShowTickets(showId: string) {
        try {
            this.showInfo = await this.service.getAllTickets(showId) as IShowInfo;
            console.log(`${this.showInfo.showId} unsold tickets`, this.showInfo.unsoldTickets)
        } catch (error) {
          alert("error getting show tickets");
          console.log(error);  
        }
    }

    async purchaseTicket(showId: string, ticketId: string) {
        try {
            const result = await this.service.bookTicket(showId, ticketId);
            if (result) {
                await this.getShowTickets(showId);
            }else {
                alert('Could not purchase ticket');
            }
        } catch (error) {
            console.log(error);
        }
    }

}

