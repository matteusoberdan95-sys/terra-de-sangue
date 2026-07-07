# Terra Sangrada - Agent Operating Guide

Este arquivo e a primeira leitura obrigatoria para qualquer agente trabalhando no projeto.

## Estado atual

- Projeto: `Terra Sangrada`.
- Engine: Godot .NET 4.7.
- Linguagem: C# / .NET 10.
- Sprint atual: Sprints 6-10 implementadas no remoto.
- Cena principal: `res://scenes/Main.tscn`.
- Proximo foco: validacao visual ao retornar.

## Ordem de leitura

1. `README.md`
2. `docs/08_estado_atual.md`
3. `docs/04_plano_de_sprints.md`
4. `docs/06_agentes.md`
5. `docs/10_politica_validacao_visual.md`
6. Documento especifico da sprint em `docs/sprints/`

Para arte e narrativa, ler tambem:

- `docs/01_biblia_de_arte.md`
- `docs/02_mundo_e_narrativa.md`
- `docs/05_pipeline_de_assets.md`

## Regras de trabalho

- Trabalhar por sprint.
- Fazer alteracoes primeiro na copia local `C:\Users\mober\OneDrive\Desktop\fauna-do-sangue`.
- Nao adicionar sistemas grandes fora do escopo da sprint atual.
- Validar visualmente no Godot antes de fechar uma sprint.
- Apos qualquer modificacao em gameplay, cena, camera, arte ou UI, registrar validacao visual conforme `docs/10_politica_validacao_visual.md`.
- So fazer commit/push depois da validacao visual local quando a alteracao afetar gameplay, cena, camera, arte ou UI.
- Nunca iniciar a proxima sprint sem commit e push da sprint validada no GitHub.
- Manter documentacao atualizada quando uma decisao mudar.
- Preservar a cultura indigena como ficticia e respeitosa ate haver pesquisa dedicada.
- Nao tratar arte final como prioridade antes do combate basico parecer bom.
- Preferir alteracoes pequenas, testaveis e faceis de reverter.

## Comandos conhecidos

```powershell
dotnet restore TerraSangrada.csproj
dotnet build TerraSangrada.csproj --no-restore
```

Observacao: no ambiente Codex, o build pode precisar de permissao para ler a configuracao NuGet do usuario.

## Gates obrigatorios

Antes de marcar uma sprint como concluida:

- Build C# sem erros.
- Cena principal abrindo.
- Validacao visual registrada.
- Checklist da sprint atualizado.
- `docs/08_estado_atual.md` atualizado.
- Commit local criado.
- Push enviado para o GitHub.

Antes de iniciar a proxima sprint:

- A sprint anterior precisa estar validada, commitada e no remoto.

## Agentes principais

- Goku: gameplay e sensacao de combate.
- Vegeta: arquitetura, seguranca tecnica e qualidade de codigo.
- Piccolo: direcao de arte e leitura visual.
- Gohan: narrativa, tom e cuidado cultural.
- Bulma: ferramentas, Godot, C# e Cursor CI.
- Trunks: continuidade, backlog e handoff entre Codex/Cursor.
- Krillin: QA, playtest e validacao de regressao.

Detalhes em `docs/06_agentes.md` e `.agents/`.
