from django.shortcuts import get_object_or_404, render
from django.views import generic
from django.http import HttpResponse, HttpResponseRedirect
from django.core.urlresolvers import reverse
from django.utils import timezone
from django.core import serializers

from .models import Choice, Question

class IndexView(generic.ListView):
    template_name = 'vote/index.html'
    context_object_name = 'latest_question_list'

    def get_queryset(self):
        """
        Return the last five published questions (not including those set to be
        published in the future).
        """
        return Question.objects.filter(
        pub_date__lte=timezone.now()
        ).order_by('-pub_date')[:5]


class DetailView(generic.DetailView):
    model = Question
    template_name = 'vote/detail.html'


class ResultsView(generic.DetailView):
    model = Question
    template_name = 'vote/results.html'
    

def vote(request, question_id):
    question = get_object_or_404(Question, pk=question_id)
    try:
        selected_choice = question.choice_set.get(pk=request.POST['choice'])
    except (KeyError, Choice.DoesNotExist):
        # Redisplay the question voting form.
        return render(request, 'vote/detail.html', {
            'question': question,
            'error_message': "You didn't select a choice.",
        })
    else:
        selected_choice.votes += 1
        selected_choice.save()
        # Always return an HttpResponseRedirect after successfully dealing
        # with POST data. This prevents data from being posted twice if a
        # user hits the Back button.
        return HttpResponseRedirect(reverse('vote:results', args=(question.id,)))
        
def ajaxRequest(request):
    """
    Responses to POST requests, fetches data from the database about the question,
    serialises it and returns it in a JSON form
    """
    if request.method == 'POST':
        question_id = request.POST.get('question_id', None)
        get_data = Choice.objects.filter(question=question_id)
        ser_data = serializers.serialize("json", get_data)

        return HttpResponse(ser_data, content_type="application/json")
    
def ajaxReply(request):
    """
    Receives data from POST requests and adds it to the database
    """
    if request.method == 'POST':
        question_name = request.POST.get('que', None)
        choice1 = request.POST.get('cho', None)
        choice2 = request.POST.get('cho2', None)
        choice3 = request.POST.get('cho3', None)
        if Question.objects.filter(question_text=question_name).exists():
                    return render(request, 'vote/index.html', {
        'error_message': "This question already exists.",
                    })
        else:            
            question = Question(question_text=question_name, pub_date=timezone.now())
            question.save()
            question = Question.objects.get(question_text=question_name)
            choice = Question.objects.get(pk=question.id)
            choice.choice_set.create(choice_text=choice1, votes=0)
            choice.choice_set.create(choice_text=choice2, votes=0)
            choice.choice_set.create(choice_text=choice3, votes=0)
            return HttpResponseRedirect(reverse('vote:detail' ,args=(question.id,)))
