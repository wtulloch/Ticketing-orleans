import { customElement, LitElement, CSSResult, css, TemplateResult, html, property } from "lit-element";
import {TicketingServices} from '../services/ticketing-service'
import { IShowData,IShowInformation } from '../models/show-info.model';

@customElement('home-view')
export class HomeView extends LitElement {

    private service: TicketingServices;

    @property({type:Array})
     shows: IShowInformation[] | undefined = undefined;
    
    @property({type: Object})
    selectedPerformance: IShowData = {
        showId: '',
        date: '',
        showName: '',
        seatAllocation: 0,
        tickets: []
    };


    @property({type: Object})
    currentShowInfo: IShowInformation | undefined;

     
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
            .show-card > p {
                text-align: center;
                margin-block-start: 0.5em;
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

            .show-dates-container {
                display: flex;
                flex-direction: row;
            }
            .date-selector {
                width: 150px;
                height: 30px;
                border: 1px black solid;
                margin: 10px;
            }
           



        `;
    }

    render(): TemplateResult {
        return html `
            <div class="show-container">
               
                ${this.shows ? 
                    this.shows.map(s => html `<div class="show-card" @click=${() => this.setCurrentShow(s)} ><p>${s.name}</p></div>`)
                    : html`<div class="show-card"> No shows yet</div>`    
            }
               
            </div>
            
            <div class="current-show-container">
        
        <div class="show-dates-container">
        ${this.currentShowInfo !== undefined ?
        html` <h2>${this.currentShowInfo.name}</h2>` 
        : html``}
       

        ${this.currentShowInfo !== undefined ? 
            this.currentShowInfo.dates.map (t => html `<div class="date-selector" @click=${() => this.getShowTickets(t)}>${t}</div>`)
        : html``}
        </div>
        <div class="ticket-container">
        ${this.selectedPerformance.tickets != null ?
            this.selectedPerformance.tickets.map(t => {
              if (t.sold){
                return html `<theatre-seat .ticket=${t}></theatre-seat>`;
                }
                return html `<theatre-seat .ticket=${t} @click=${() => this.onTicketClick(t.ticketId)}></theatre-seat>`
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

    setCurrentShow(show: IShowInformation) {
        this.currentShowInfo = show;
        this.selectedPerformance = {
            showId: '',
            date: '',
            showName: '',
            seatAllocation: 0,
            tickets: []
        };
    }

    async getShowTickets(date: string) {
        try {
            this.selectedPerformance = await this.service.getAllTickets(this.currentShowInfo!.baseShowId, date) as IShowData;
            console.log(`${this.selectedPerformance.showId}`);
        } catch (error) {
          alert("error getting show tickets");
          console.log(error);  
        }
    }

    async purchaseTicket(showId: string, ticketId: string) {
        try {
            const result = await this.service.bookTicket(showId, ticketId);
            if (result) {
                await this.getShowTickets(this.selectedPerformance!.date);
            }else {
                alert('Could not purchase ticket');
            }
        } catch (error) {
            console.log(error);
        }
    }

    private async onTicketClick(ticketId: string) {
        await this.purchaseTicket(this.selectedPerformance.showId, ticketId);
    }

}

